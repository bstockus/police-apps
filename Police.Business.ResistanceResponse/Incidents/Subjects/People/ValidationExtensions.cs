using FluentValidation;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.People {

    public static class ValidationExtensions {
        
        public static void AsPersonSubjectFullName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(200);

        public static void AsPersonSubjectAge<T>(this IRuleBuilder<T, int?> ruleBuilder) =>
            ruleBuilder.GreaterThan(0).LessThan(120);

        public static void AsPersonSubjectGender<T>(this IRuleBuilder<T, Gender> ruleBuilder) =>
            ruleBuilder.NotNull();

        public static void AsPersonSubjectRace<T>(this IRuleBuilder<T, Race> ruleBuilder) =>
            ruleBuilder.NotNull();

        public static void AsPersonSubjectSuspectedUse<T>(
            this IRuleBuilder<T, SuspectedUse> ruleBuilder) =>
            ruleBuilder.NotNull();

    }

}