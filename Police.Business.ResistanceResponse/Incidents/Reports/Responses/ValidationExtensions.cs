using FluentValidation;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public static class ValidationExtensions {

        public static void AsResponseType<T>(this IRuleBuilder<T, ResponseType> ruleBuilder) =>
            ruleBuilder.NotNull();

        public static void AsFireArmMake<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(100);

        public static void AsFireArmModel<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(100);

        public static void AsFireArmSerialNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(100);

        public static void AsFireArmAmmoType<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(100);

        public static void AsFireArmDescription<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(1000);

        public static void AsOtherDeadlyForceDescription<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(1000);

        public static void AsPitUsageVehicleSpeed<T>(this IRuleBuilder<T, int> ruleBuilder) =>
            ruleBuilder.NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(250);

        public static void AsTaserSerialNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(50);

        public static void AsTaserCyclesApplied<T>(this IRuleBuilder<T, int> ruleBuilder) =>
            ruleBuilder.NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(20);

        public static void AsTaserDistanceWhenLaunched<T>(this IRuleBuilder<T, decimal> ruleBuilder) =>
            ruleBuilder.NotNull().GreaterThanOrEqualTo(0.00m).LessThanOrEqualTo(100.00m);

        public static void AsTaserDistanceBetweenProbes<T>(this IRuleBuilder<T, decimal> ruleBuilder) =>
            ruleBuilder.NotNull().GreaterThanOrEqualTo(0.00m).LessThanOrEqualTo(48.00m);

        public static void AsTaserWhoRemovedProbes<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(1000);

        public static void AsTaserNumberOfPhotosTaken<T>(this IRuleBuilder<T, int> ruleBuilder) =>
            ruleBuilder.NotNull().GreaterThanOrEqualTo(0);

        public static void AsTaserCameraUsedToTakePhotos<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(200);

        public static void AsTaserCartridgeNumberUsed<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(50);

        public static void AsTaserBodyUsageLocationPoint<T>(this IRuleBuilder<T, int> ruleBuilder) =>
            ruleBuilder.NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(500);

    }

}