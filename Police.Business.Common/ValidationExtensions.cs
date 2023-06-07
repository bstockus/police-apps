using System;
using FluentValidation;

namespace Police.Business.Common {

    public static class ValidationExtensions {
        
        public static void AsYesNoDescription<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotNull().MaximumLength(1000);

        public static void AsEntityIdentity<T>(this IRuleBuilder<T, Guid> ruleBuilder) =>
            ruleBuilder.NotEmpty();

    }

}