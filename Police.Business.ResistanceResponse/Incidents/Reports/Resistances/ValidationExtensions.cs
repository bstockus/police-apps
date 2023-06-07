using FluentValidation;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Resistances {

    public static class ValidationExtensions {

        public static void AsResistanceType<T>(this IRuleBuilder<T, ResistanceType> ruleBuilder) =>
            ruleBuilder.NotNull();

        public static void AsResistanceEncounteredDescription<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(1000);

    }

}