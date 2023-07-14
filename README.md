# Edge Present #

A .net MAUI application to present content managed by [Sitecore Content Hub ONE](https://www.sitecore.com/products/content-hub-one) and queried from [Sitecore Experience Edge](https://doc.sitecore.com/xmc/en/developers/xm-cloud/sitecore-experience-edge-for-xm.html).

Refer to the [release/content-hub](https://github.com/adeneys/EdgeContentPresenter/tree/release/content-hub) branch for a version of this application I used at SUGCON ANZ 2022 which showed content from [Sitecore Content Hub CMP](https://docs.stylelabs.com/contenthub/4.2.x/content/user-documentation/manage/cmp/cmp.html).

Although this application can currently run on mobile devices, it's layout is designed for full screen use on a desktop or laptop computer.

This application is an example of how content can be queried from Experience Edge without the need for any additional service.

## Content ##

The content for the application is managed in [Sitecore Content Hub ONE](https://www.sitecore.com/products/content-hub-one). The [Content Hub ONE CLI](https://doc.sitecore.com/ch-one/en/developers/content-hub-one/content-hub-one-cli.html) is used to manage the Content Types and Content.

To import the content into a Content Hub ONE tenant, first [Install the Content Hub ONE CLI](https://doc.sitecore.com/ch-one/en/developers/content-hub-one/content-hub-one-cli--install-and-run-the-cli.html) and [Log in with the CLI](https://doc.sitecore.com/ch-one/en/developers/content-hub-one/content-hub-one-cli--log-in-with-the-cli.html). Now the `serialization` command can be used to push resources to Content Hub ONE. Execute the following command from the `chone` directory of the repository:

	dotnet ch-one-cli serialization push content-type
	dotnet ch-one-cli serialization push content-item
