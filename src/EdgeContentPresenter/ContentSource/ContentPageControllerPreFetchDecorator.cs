﻿using EdgeContentPresenter.ContentTypes;
using EdgeContentPresenter.Model;
using System.ComponentModel;

namespace EdgeContentPresenter.ContentSource
{
    /// <summary>
    ///  A decorator for IContentPageController which loads the next page when a new page is loaded.
    /// </summary>
    internal class ContentPageControllerPreFetchDecorator : IContentPageController
    {
        private readonly IContentPageController _inner;
        private readonly IContentRepository _contentRepository;
        private readonly IContentPageFactory _contentPageFactory;

        public IList<NavigablePage>? NavigablePages => _inner.NavigablePages;
        public Page CurrentPage => _inner.CurrentPage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContentPageControllerPreFetchDecorator(IContentPageController inner, IContentRepository contentRepository, IContentPageFactory contentPageFactory)
        {
            _inner = inner;
            _contentRepository = contentRepository;
            _contentPageFactory = contentPageFactory;

            _inner.PropertyChanged += InnerPropertyChanged;
        }

        public Task LoadNavigation(string name)
        {
            return _inner.LoadNavigation(name);
        }

        public Task LoadContent(string identifier)
        {
            return _inner.LoadContent(identifier);
        }

        private async void InnerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));

            if (e.PropertyName == nameof(IContentPageController.CurrentPage))
            {
                if (CurrentPage.BindingContext is Content content)
                {
                    var nextPage = _inner.NavigablePages.FindNextPage(content.Identifier);

                    if (nextPage != null)
                    {
                        var nextContent = await _contentRepository.GetContentAsync(nextPage.Type, nextPage.Identifier);
                        _contentPageFactory.CreatePageForContent(nextContent);
                    }
                }
            }
        }
    }
}
