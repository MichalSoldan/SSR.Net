using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using SSR.Net.Services;

namespace SSR.Net.DotNet8.Controllers
{
    public class HomeController : Controller
    {
        private readonly React17Renderer _react17Renderer;
        private readonly React18Renderer _react18Renderer;
        private readonly React19Renderer _react19Renderer;
        private readonly Vue3Renderer _vue3Renderer;

        public HomeController(React17Renderer react17Renderer,
                              React18Renderer react18Renderer,
                              React19Renderer react19Renderer,
                              Vue3Renderer vue3Renderer)
        {
            _react17Renderer = react17Renderer;
            _react18Renderer = react18Renderer;
            _react19Renderer = react19Renderer;
            _vue3Renderer = vue3Renderer;
        }

        public ActionResult Index() => View();

        public ActionResult React17()
        {
            var propsJson = JsonConvert.SerializeObject(
                new
                {
                    header = "React 17 with SSR",
                    links = new[]{
                        new {
                            text = "Google.com",
                            href ="https://www.google.com"
                        },
                        new {
                            text = "Hacker news",
                            href = "https://news.ycombinator.org"
                        }
                    }
                });
            var renderedComponent = _react17Renderer.RenderComponent("Components.FrontPage", propsJson);
            return base.View(renderedComponent);
        }

        public ActionResult React18()
        {
            var propsJson = JsonConvert.SerializeObject(
                new
                {
                    header = "React 18 with SSR",
                    links = new[]{
                        new {
                            text = "Google.com",
                            href = "https://www.google.com"
                        },
                        new {
                            text = "Hacker news",
                            href = "https://news.ycombinator.org"
                        }
                    }
                });
            var renderedComponent = _react18Renderer.RenderComponent("Components.FrontPage", propsJson);
            return View(renderedComponent);
        }

        public ActionResult React19()
        {
            //var propsJson = JsonConvert.SerializeObject(
            //    new {
            //        header = "React 19 with SSR",
            //        links = new[]{
            //            new {
            //                text = "Google.com",
            //                href = "https://www.google.com"
            //            },
            //            new {
            //                text = "Hacker news",
            //                href = "https://news.ycombinator.org"
            //            }
            //        }
            //    });
            //var renderedComponent = _react19Renderer.RenderComponent("Components.FrontPage", propsJson);

            var propsJson = new
            {
                //general = new
                //{
                //    currentUrl = "http =//www.example.com/",
                //    breakpoint = "screenXs",
                //    culture = "cs",
                //    localizations = new Dictionary<string, string>
                //    {
                //        { "carousel/previousSlide", "Previous slide" },
                //        { "carousel/nextSlide", "Next slide" },
                //        { "modal/close", "Close modal" },
                //        { "error/generic", "Something went wrong." },
                //    },
                //    urls = new
                //    {
                //        homepageUrl = "/",
                //        signInUrl = "/account/signin",
                //        registerUrl = "/account/registration",
                //        userSettingsUrl = "/account/settings",
                //        logoutRedirectUrl = "/",
                //    },
                //    isPreviewMode = false,
                //    auth = new
                //    {
                //        loggedIn = false,
                //        firstName = "",
                //    },
                //    cartItems = 0,
                //    notification = new
                //    {
                //        type = "error",
                //        title = "Example Optional title",
                //        message = "Example Error notification message",
                //    },
                //    requestVerificationToken = "KenticoRequestVerificationToken",
                //    reCaptchaSiteKey = "6LfPq-IUAAAAABUDAjWdsv4MXaTVqL9KkByGudvu",

                //},
                config = new
                {
                    id = "guid-card",
                    title = "Test of CTA",
                    url = "/some-url",
                    target = "_self",
                    text = "Lorem ipsum dolor sit amet. ",
                    image = new
                    {
                        alt = "Nice image",
                        defaultUrl = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7",
                        defaultUrlHiRes = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7",
                        sources = new[]
                    {
                            new
                            {
                                view = 980,
                                url = "//satyr.dev/462x148/19?text=tablet&delay=500-800",
                                urlHiRes = "//satyr.dev/924x296/19?text=tablet&delay=500-800",
                            },
                            new
                            {
                                view = 1280,
                                url = "//satyr.dev/692x148/19?text=desktop&delay=500-800",
                                urlHiRes = "//satyr.dev/1384x296/19?text=desktop&delay=500-800",
                            },
                        },
                    }
                }
            };

            var renderedComponent = _react19Renderer.RenderComponent("Components.Cta", JsonConvert.SerializeObject(propsJson));
            return View(renderedComponent);
        }

        public ActionResult Vue3()
        {
            var propsJson = JsonConvert.SerializeObject(
                new
                {
                    title = "Vue 3 with SSR"
                });
            var renderedComponent = _vue3Renderer.RenderComponent("Components.Example", propsJson);
            return View(renderedComponent);
        }
    }
}