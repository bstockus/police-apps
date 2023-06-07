using FluentValidation;
using NodaTime;

namespace Police.Business.ResistanceResponse.Incidents {

    public static class ValidationExtensions {

        public static void AsIncidentCaseNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(10);

        public static void AsIncidentDateAndTime<T>(this IRuleBuilder<T, LocalDateTime> ruleBuilder) =>
            ruleBuilder.NotNull();

    }

}