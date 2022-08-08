using EdgeContentPresenter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeContentPresenter.ContentSource
{
    internal interface IContentMapper
    {
        Content? MapContentResponse(string content);
    }
}
