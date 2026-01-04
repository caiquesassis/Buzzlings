using Buzzlings.BusinessLogic.Models.Enums;

namespace Buzzlings.BusinessLogic.Simulation
{
    public static class SimulationEventLogs
    {
        private static readonly List<string> BuzzlingCreationMotivatorWorkerLogs = new List<string>
        {
            "⚠️ Tasks are piling up. Too few Workers to handle it all.",
            "⚠️ Hive construction is slowing...too many jobs, not enough Workers.",
            "⚠️ Building activity is slow. The hive’s output is dipping.",
            "⚠️ Tasks are delayed. More Workers could help ease the burden.",
            "⚠️ The buzz is quiet. Work is falling behind."
        };

        private static readonly List<string> BuzzlingCreationMotivatorGuardLogs = new List<string>
        {
            "⚠️ The hive’s defenses feel thin. A shadow lingers.",
            "⚠️ Unease rises — more protection is needed.",
            "⚠️ The peace is fragile...security feels weak.",
            "⚠️ The Guards are overwhelmed. Vigilance is needed.",
            "⚠️ Buzzing grows anxious — security is lacking."
        };

        private static readonly List<string> BuzzlingCreationMotivatorForagerLogs = new List<string>
        {
            "⚠️ Nectar supplies are low. Foragers can’t keep up.",
            "⚠️ Food reserves are dwindling. More Foragers needed.",
            "⚠️ Nectar’s scent is faint. Food sources are running dry.",
            "⚠️ Foragers are few. Nectar is hard to find.",
            "⚠️ Food stores are shrinking. Gathering is slower."
        };

        private static readonly List<string> BuzzlingCreationMotivatorNurseLogs = new List<string>
        {
            "⚠️ The larvae are growing fast. Not enough care to go around.",
            "⚠️ The brood is at risk. Nurses can’t keep up.",
            "⚠️ The sick aren’t getting enough attention.",
            "⚠️ The brood’s needs are growing, and Nurses are stretched thin.",
            "⚠️ Some larvae are neglected. Nurses have too much to handle."
        };

        private static readonly List<string> BuzzlingCreationMotivatorAttendantLogs = new List<string>
        {
            "⚠️ The Queen is stressed. More Attendants could help.",
            "⚠️ The Queen’s comfort is slipping — attention is lacking.",
            "⚠️ The Queen’s mood is unstable — she has few Attendants.",
            "⚠️ The Queen buzzes in frustration. Too few Attendants.",
            "⚠️ The Queen feels distant — Attendants are scarce."
        };

        private static readonly List<string> BuzzlingCreationMotivatorDroneLogs = new List<string>
        {
            "⚠️ The Queen is restless. Drones are too few.",
            "⚠️ The Queen’s mood is dropping. Not enough Drones near her.",
            "⚠️ The Queen’s comfort is waning. Drones are needed.",
            "⚠️ The Queen’s discontent rises. Drones could help.",
            "⚠️ The Queen’s spirits are low — too few Drones to keep her happy."
        };

        private static readonly List<string> HiveDisasterWeatherLogs = new List<string>
        {
            "⛈️ A storm hit hard. A few buzzlings fled the hive to escape the chaos.",
            "🔥 A heat wave scorched the hive. Some buzzlings fled to cooler areas.",
            "❄️ A cold front arrived. Some buzzlings fled, leaving the hive vulnerable.",
            "⛈️ A storm caused major damage. A few buzzlings fled the hive for safety.",
            "🔥 The sun is too hot, damaging the hive. A few buzzlings fled the heat.",
            "❄️ The cold has spread. Some buzzlings fled, the hive can't stay warm."
        };

        private static readonly List<string> HiveDisasterAttackLogs = new List<string>
        {
            "🚨 Wasps attacked. Some buzzlings fled, leaving the hive exposed.",
            "🚨 Ants invaded the area. Some buzzlings fled, weakening the hive.",
            "🚨 A curious child disrupted the hive. Some buzzlings fled in fear.",
            "🚨 Intruders attacked the hive. Some buzzlings fled, leaving the queen.",
            "🚨 The hive was under siege. Some buzzlings fled in panic."
        };

        private static readonly List<string> HiveDisasterDiseaseOutbreakLogs = new List<string>
        {
            "🤒 A disease spread through the hive. Some buzzlings fled the sickness.",
            "🤒 A sickness spread. Some buzzlings fled the hive to escape infection.",
            "🤒 Many buzzlings are sick. A few fled to find a healthier place.",
            "🤒 A disease took its toll. A few buzzlings fled the hive for safety.",
            "🤒 The hive is infected. Some buzzlings fled, leaving the brood at risk."
        };

        private static readonly List<string> HiveDisasterNectarShortageLogs = new List<string>
        {
            "🍯 Nectar reserves are low. Some buzzlings fled in search of food.",
            "🍯 The food supply is running out. Buzzlings fled to find more nectar.",
            "🍯 Nectar sources dried up. A few buzzlings fled to look for new food.",
            "🍯 The hive is starving. Some buzzlings fled to find nectar elsewhere.",
            "🍯 Food supplies are critical. A few buzzlings fled to other hive."
        };

        private static readonly List<string> HiveDisasterQueenUnhappyLogs = new List<string>
        {
            "👑 The Queen’s mood dropped. A few buzzlings fled to avoid her anger.",
            "👑 The Queen is upset. Buzzlings fled in search of a more peaceful place.",
            "👑 The Queen feels neglected. Some buzzlings fled the hive in turmoil.",
            "👑 The Queen’s discomfort is growing. Some buzzlings fled in fear.",
            "👑 The Queen is in a bad mood. Some buzzlings were cast out."
        };

        private static readonly List<string> SingleBuzzlingSelfNameLogs = new List<string>
        {
            " dislikes their name. Restlessness grows.",
            " hates their name. Frustration builds.",
            " is unhappy with their name. Tension rises.",
            " feels overlooked. Mood is shifting.",
            " hates their name. Focus is fading."
        };

        private static readonly List<string> SingleBuzzlingSelfRoleLogs = new List<string>
        {
            " feels bored with their role. Restlessness grows.",
            " is stuck. Work is losing meaning.",
            " feels trapped. Motivation is dropping.",
            " is dissatisfied with their tasks. Productivity drops.",
            " feels wasted. They’re disengaging."
        };

        private static readonly List<string> SingleBuzzlingRivalNameLogs = new List<string>
        {
            " is jealous of RIVAL’s name. Tension rises.",
            " feels overlooked. Frustration grows.",
            " envies RIVAL’s name. Resentment builds.",
            " is angry at RIVAL’s name. Mood soured.",
            " is upset by RIVAL’s name. Rivalry grows."
        };

        private static readonly List<string> SingleBuzzlingRivalRoleLogs = new List<string>
        {
            " envies RIVAL’s role. Tension rises.",
            " is frustrated by RIVAL’s position. Resentment builds.",
            " is angry that RIVAL has more power. Rivalry deepens.",
            " feels undermined by RIVAL’s role. Conflict grows.",
            " is jealous of RIVAL’s role. Productivity drops."
        };

        private static readonly List<string> SingleBuzzlingWorkerActionLogs = new List<string>
        {
            " buzzes impatiently. Boredom is setting in.",
            " breaks a wall out of boredom. Hive feels strained.",
            " grumbles. Too many Workers, not enough work.",
            " feels overlooked. Resentment grows.",
            " complains. Idle time is rising."
        };

        private static readonly List<string> SingleBuzzlingGuardActionLogs = new List<string>
        {
            " accuses another of treason. Tensions rise.",
            " grows aggressive. The hive is on edge.",
            " challenges a buzzling. Loyalty questioned.",
            " snaps at a buzzling. Tension mounts.",
            " buzzes nervously. They're on high alert."
        };

        private static readonly List<string> SingleBuzzlingForagerActionLogs = new List<string>
        {
            " returns empty-handed. Frustration grows.",
            " finds food, but no one appreciates it.",
            " returns with nectar, but no help arrives.",
            " scouts the area. Food is scarce.",
            " struggles to find new food. The hive is hungry."
        };

        private static readonly List<string> SingleBuzzlingNurseActionLogs = new List<string>
        {
            " is overwhelmed. Some larvae are neglected.",
            " buzzes nervously. Too many larvae, not enough care.",
            " seems distracted. Some larvae are unattended.",
            " is exhausted. The larvae need more attention.",
            " feels burdened. Not enough Nurses for the brood."
        };

        private static readonly List<string> SingleBuzzlingAttendantActionLogs = new List<string>
        {
            " glares at the Queen. Feeling ignored.",
            " is growing impatient. Frustration is rising.",
            " feels overlooked. Frustration builds.",
            " snaps at another. Competition for the Queen grows.",
            " huffs in irritation. Others are too close to the Queen."
        };

        private static readonly List<string> SingleBuzzlingDroneActionLogs = new List<string>
        {
            " lounges near the Queen. Frustration growing.",
            " feels useless. Resentment builds.",
            " sighs in frustration. Idle time is too long.",
            " feels guilty. Consumes honey, but contributes nothing.",
            " grumbles. Tired of doing nothing while others work."
        };

        private static readonly List<string> SingleBuzzlingMoodLogs = new List<string>
        {
            "'s bad mood spread. Some became nervous.",
            "’s frustration made others uneasy.",
            "’s anger caused unrest. Conflicts are rising.",
            "’s low spirits spread. Some distanced to avoid conflict.",
            "’s mood soured. It's affecting some in the hive."
        };

        public static readonly List<string> DecorationLogs = new List<string>
        {
            "🌱 The hive hums softly as buzzlings go about their work.",
            "🎶 A buzzling is buzzing loudly, filling the hive with a cheerful sound.",
            "🌞 The sun shines through, casting a warm glow over the hive.",
            "🍃 A gentle breeze stirs the leaves outside, bringing calmness to the hive.",
            "🧹 A buzzling quietly polishes the walls, humming to themselves.",
            "🌸 The hive is peaceful, with buzzlings drifting in and out of their duties.",
            "🍯 A buzzling buzzes happily as they carry a tiny load of nectar.",
            "🌤️ A buzzling pauses for a moment, staring at the sky outside.",
            "🛏️ The Queen rests in her chamber, the buzzing of attendants in the air.",
            "🌼 A buzzling returns, and the scent of fresh pollen fills the hive.",
            "💤 A buzzling stretches and yawns, enjoying a rare moment of rest.",
            "🦗 The air in the hive is still, with only the occasional buzzling passing by.",
            "☕ A buzzling gives a lazy stretch, and then goes back to their task.",
            "🌿 The hive feels calm, a quiet buzz filling the air as the day passes.",
            "💭 A buzzling hums softly, lost in thought as they carry out their task."
        };

        public static readonly Dictionary<SimulationBuzzlingCreationMotivatorEventType, List<string>> BuzzlingCreationMotivatorLogs = new Dictionary<SimulationBuzzlingCreationMotivatorEventType, List<string>>()
        {
            { SimulationBuzzlingCreationMotivatorEventType.Worker, BuzzlingCreationMotivatorWorkerLogs },
            { SimulationBuzzlingCreationMotivatorEventType.Guard, BuzzlingCreationMotivatorGuardLogs },
            { SimulationBuzzlingCreationMotivatorEventType.Forager, BuzzlingCreationMotivatorForagerLogs },
            { SimulationBuzzlingCreationMotivatorEventType.Nurse, BuzzlingCreationMotivatorNurseLogs },
            { SimulationBuzzlingCreationMotivatorEventType.Attendant, BuzzlingCreationMotivatorAttendantLogs },
            { SimulationBuzzlingCreationMotivatorEventType.Drone, BuzzlingCreationMotivatorDroneLogs }
        };

        public static readonly Dictionary<SimulationHiveDisasterEventType, List<string>> HiveDisasterLogs = new Dictionary<SimulationHiveDisasterEventType, List<string>>
        {
            { SimulationHiveDisasterEventType.Weather, HiveDisasterWeatherLogs },
            { SimulationHiveDisasterEventType.Attack, HiveDisasterAttackLogs },
            { SimulationHiveDisasterEventType.DiseaseOutbreak, HiveDisasterDiseaseOutbreakLogs },
            { SimulationHiveDisasterEventType.NectarShortage, HiveDisasterNectarShortageLogs },
            { SimulationHiveDisasterEventType.QueenUnhappy, HiveDisasterQueenUnhappyLogs }
        };

        public static readonly Dictionary<SimulationSingleBuzzlingEventType, List<string>> SingleBuzzlingLogs = new Dictionary<SimulationSingleBuzzlingEventType, List<string>>
        {
            { SimulationSingleBuzzlingEventType.SelfName, SingleBuzzlingSelfNameLogs },
            { SimulationSingleBuzzlingEventType.SelfRole, SingleBuzzlingSelfRoleLogs },
            { SimulationSingleBuzzlingEventType.RivalName, SingleBuzzlingRivalNameLogs },
            { SimulationSingleBuzzlingEventType.RivalRole, SingleBuzzlingRivalRoleLogs },
            { SimulationSingleBuzzlingEventType.Mood, SingleBuzzlingMoodLogs }
        };

        public static readonly Dictionary<SimulationSingleBuzzlingActionEventType, List<string>> SingleBuzzlingActionLogs = new Dictionary<SimulationSingleBuzzlingActionEventType, List<string>>()
        {
            { SimulationSingleBuzzlingActionEventType.Worker, SingleBuzzlingWorkerActionLogs },
            { SimulationSingleBuzzlingActionEventType.Guard, SingleBuzzlingGuardActionLogs },
            { SimulationSingleBuzzlingActionEventType.Forager, SingleBuzzlingForagerActionLogs },
            { SimulationSingleBuzzlingActionEventType.Nurse, SingleBuzzlingNurseActionLogs },
            { SimulationSingleBuzzlingActionEventType.Attendant, SingleBuzzlingAttendantActionLogs },
            { SimulationSingleBuzzlingActionEventType.Drone, SingleBuzzlingDroneActionLogs }
        };
    }
}
