using Lax.Helpers.Common;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public static class ResponseTypeExtensions {

        public static bool IsFireArmDeadlyForceAddendumRequired(this ResponseType responseType) =>
            responseType.GetAttributeOfType<FireArmDeadlyForceAddendum.FireArmDeadlyForceAddendumRequiredAttribute>() !=
            null;

        public static bool IsOtherDeadlyForceAddendumRequired(this ResponseType responseType) =>
            responseType.GetAttributeOfType<OtherDeadlyForceAddendum.OtherDeadlyForceAddendumRequiredAttribute>() !=
            null;

        public static bool IsPitUsageAddendumRequired(this ResponseType responseType) =>
            responseType.GetAttributeOfType<PitUsageAddendum.PitUsageAddendumRequiredAttribute>() !=
            null;

        public static bool IsTaserAddendumRequired(this ResponseType responseType) =>
            responseType.GetAttributeOfType<TaserUsageAddendum.TaserUsageAddendumRequiredAttribute>() !=
            null;

    }

}