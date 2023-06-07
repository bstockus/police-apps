﻿using Microsoft.AspNetCore.Mvc.Razor;
using Police.Web.Common.Constants;

namespace Police.Web.Common.Infrastructure {

    public static class LayoutExtensions {

        public static void UseLayout(this RazorPageBase razorPageBase, string title) {
            razorPageBase.ViewBag.Title = title;
            razorPageBase.Layout = Layouts.Layout;
        }

        public static void UseModalLayout(this RazorPageBase razorPageBase, string title) {
            razorPageBase.ViewBag.Title = title;
            razorPageBase.Layout = Layouts.ModalLayout;
        }

    }

}