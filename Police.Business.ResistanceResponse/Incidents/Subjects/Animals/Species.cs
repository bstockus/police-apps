using FluentValidation;

namespace Police.Business.ResistanceResponse.Incidents.Subjects.Animals {

    public enum Species {

        Other = 0,
        Bear = 1,
        Deer = 2

    }

    public static class ValidationExtensions {

        public static void AsAnimalSubjectSpecies<T>(this IRuleBuilder<T, Species> ruleBuilder) =>
            ruleBuilder.NotNull();

    }

}