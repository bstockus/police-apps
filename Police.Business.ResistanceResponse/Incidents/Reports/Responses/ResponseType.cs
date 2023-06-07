using System.ComponentModel.DataAnnotations;

namespace Police.Business.ResistanceResponse.Incidents.Reports.Responses {

    public enum ResponseType {

        Decenteralization = 1,
        [Display(Name = "Vertical Stun")] VerticalStun = 2,

        [Display(Name = "Hand or Elbow Strike")]
        HandOrElbowStrike = 3,
        [Display(Name = "Kick or Knee")] KickOrKnee = 4,
        OC = 5,

        [Display(Name = "Lateral Vascular Restraint")]
        LateralVascularRestraint = 6,
        BrachialStun = 7,
        [Display(Name = "Taser"), TaserUsageAddendum.TaserUsageAddendumRequired]
        Taser = 8,
        [Display(Name = "Impact Weapon")] ImpactWeapon = 12,
        [Display(Name = "Less Leathal")] LessLeathalProjectile = 13,
        [Display(Name = "Tear Gas")] TearGas = 14,

        [PitUsageAddendum.PitUsageAddendumRequired]
        PIT = 15,

        [Display(Name = "Deadly Force - Firearm"), FireArmDeadlyForceAddendum.FireArmDeadlyForceAddendumRequired]
        FirearmDeadlyForce = 16,

        [Display(Name = "Deadly Force - Other"), OtherDeadlyForceAddendum.OtherDeadlyForceAddendumRequired]
        OtherDeadlyForce = 17

    }

}