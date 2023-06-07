using FluentValidation;

namespace Police.Business.ResistanceResponse.Approvals {

    public static class ValidationExtensions {

        public static void AsApproversComments<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.MaximumLength(1000);

    }

}