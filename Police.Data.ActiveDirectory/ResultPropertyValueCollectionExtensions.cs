using System.DirectoryServices;
using System.Security.Principal;

namespace Police.Data.ActiveDirectory {

    internal static class ResultPropertyValueCollectionExtensions {

        internal static string GetFirstStringValue(this ResultPropertyValueCollection @this) {
            if (@this == null) {
                return "";
            }

            return @this.Count > 0 ? @this[0].ToString() : "";
        }

        internal static string GetFirstSidStringValue(this ResultPropertyValueCollection @this) {
            if (@this == null) {
                return "";
            }

            return @this.Count > 0 ? (new SecurityIdentifier((byte[]) @this[0], 0)).ToString() : "";
        }

    }

}