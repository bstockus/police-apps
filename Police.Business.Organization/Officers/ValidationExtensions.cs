 using FluentValidation;

namespace Police.Business.Organization.Officers {

    public static class ValidationExtensions {

        public static void AsOfficerEmployeeNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(8);

        public static void AsOfficerBadgeNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(4);

        public static void AsOfficerRank<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(50);

        public static void AsOfficerJobTitle<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(200);

        public static void AsOfficerAssignment<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(200);

        public static void AsOfficerFirstName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(100);

        public static void AsOfficerLastName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(200);

    }

}