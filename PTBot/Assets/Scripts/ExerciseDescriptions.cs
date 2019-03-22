using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonData;

[SerializeField]
public class ExerciseDescriptions
{
    // Arm exercise video URLs
    public static string AlternateDumbbellCurls = "Alternate Dumbbell Curls\n\n1. Stand up straight with a dumbbell in each hand. Keep upper arms stationary.\n2. Exhale and curl left hand dumbbell while contracting your left bicep.Rotate the dumbbell during this motion so your palms are facing towards your upper body.\n3. When fully contracted squeeze your bicep, then lower the weight back down to the starting position as you inhale.\n4. Repeat with right arm.\n5. Continue alternating arms for desired amount of repetitions.\n";
    public static string AlternateDumbbellCurlsIncline = "Alternate Dumbbell Curls Incline\n\n1. Sit down on incline bench with dumbbells held in each hand at arm’s length.\n2. Exhale and curl left hand dumbbell while contracting your left bicep.\n3. When fully contracted squeeze your bicep, then lower the weight back down to the starting position as you inhale.\n4. Repeat with right arm.\n5. Continue alternating arms for desired amount of repetitions.\n";
    public static string BarbellCurls = "Barbell Curls\n\n1. Stand upright and hold the barbell at a shoulder-width grip.\n2. Hold upper arms stationary, curl the barbell and contract whilst exhaling.\n3. Squeeze the biceps when contracted.\n4. Inhale whilst lowering the barbell to the starting position.\n5. Repeat for desired amount of repetitions.\n";
    public static string BarbellCurlsIncline = "Barbell Curls Incline\n\n1. Lie against an incline bench holding the barbell hanging down.\n2. Hold upper arms stationary, curl the barbell and contract whilst exhaling.\n3. Squeeze the biceps when contracted.\n4. Inhale whilst lowering the barbell to the starting position.\n5. Repeat for desired number of repetitions.\n";
    public static string ConcentrationCurls = "Contentration Curls\n\n1. Sit on a flat bench and hold a dumbbell between your legs.\n2. Use your left arm to pick the dumbbell up and place your upper left arm on your inner left thigh. The palm of your left hand should face away from your left thigh.\n3. Hold the upper arm stationary and curl the dumbbell whilst exhaling and contracting the bicep.\n4. Squeeze the bicep when contracted.\n5. Lower the dumbbell to the starting position as you inhale.\n6. Repeat for desired number of repetitions, then repeat with your right arm.\n";
    public static string HammerCurls = "Hammer Curls\n\n1. Stand up straight with a dumbbell in each hand. Keep upper arms stationary. The palms of your hands should face your torso.\n2. Exhale and curl left hand dumbbell while contracting your left bicep.\n3. When fully contracted squeeze your bicep, then lower the weight back down to the starting position as you inhale.\n4. Repeat with right arm.\n5. Continue alternating arms for desired amount of repetitions.\n";
    public static string TricepPushdowns = "Tricep Pushdowns\n\n1. Attach either a straight / angled bar or a rope to a high cable pulley machine.\n2. Grab the bar with your palms facing down, and stand slightly leaning forward.\n3. Exhale whilst bringing the bar down by contracting your triceps until your arms are fully extended perpendicular to the floor.\n4. Hold the bar whilst fully contracted and slowly bring up the bar and inhale during this motion.\n5. Repeat for desired amount of repetitions.\n";
    public static string TricepDips = "Tricep Dips\n\n1. Place a bench behind your back.\n2. Face away from the bench and hold onto the bench your hands fully extended and separated at shoulder width.\n3. Extend your legs forward perpendicular to your torso.\n4. Slowly inhale and lower your body.\n5. Contract your triceps to lift yourself up again.\n6. Repeat for desired amount of repetitions.\n";

    // Back exercise video URLs
    public static string BarbellShrug = "Barbell Shrugs\n\n1. Stand upright and hold the barbell with both hands and your palms facing your thighs.\n2. Exhale and lift the barbell with only your shoulders. The biceps shouldn’t help lift the barbell for this exercise and your arms should remain extended at all times.\n3. Lower the barbell back to the starting position.\n4. Repeat for desired number of repetitions.\n";
    public static string DumbbellShrug = "Dumbbell Shrug\n\n1. Stand with a dumbbell in each hand with your palms facing your torso.\n2. Exhale and lift the dumbbells with only your shoulders. The biceps shouldn’t help lift the dumbbells for this exercise and your arms should remain extended at all times.\n3. Lower the dumbbells back to the starting position.\n4. Repeat for desired number of repetitions.\n";
    public static string LatPulldowns = "https://videos.bodybuilding.com/video/mp4/32000/32081m.mp4";
    public static string Pullups = "https://videos.bodybuilding.com/video/mp4/30000/30171m.mp4";
    public static string SeatedCableRows = "https://videos.bodybuilding.com/video/mp4/30000/30431m.mp4";

    // Chest exercise video URLs
    public static string BarbellBenchPress = "https://videos.bodybuilding.com/video/mp4/54000/54651m.mp4";
    public static string DumbbellBenchPressDecline = "https://videos.bodybuilding.com/video/mp4/32000/32131m.mp4";
    public static string DumbbellBenchPress = "https://videos.bodybuilding.com/video/mp4/28000/28871m.mp4";
    public static string DumbbellFlyes = "https://videos.bodybuilding.com/video/mp4/28000/28921m.mp4";
    public static string Pushups = "https://videos.bodybuilding.com/video/mp4/30000/30191m.mp4";

    // Core exercise video URLs
    public static string AirBikes = "https://videos.bodybuilding.com/video/mp4/24000/25571m.mp4";
    public static string BarbellSideBends = "https://videos.bodybuilding.com/video/mp4/24000/25781m.mp4";
    public static string DumbbellSideBends = "https://videos.bodybuilding.com/video/mp4/28000/29141m.mp4";
    public static string HangingLegRaises = "https://videos.bodybuilding.com/video/mp4/28000/29401m.mp4";
    public static string RussianTwists = "https://videos.bodybuilding.com/video/mp4/30000/30361m.mp4";
    public static string SeatedBarbellTwists = "https://videos.bodybuilding.com/video/mp4/30000/30391m.mp4";

    // Leg exercise video URLs
    public static string BarbellDeadlift = "https://videos.bodybuilding.com/video/mp4/118000/118911m.mp4";
    public static string BarbellLunges = "https://videos.bodybuilding.com/video/mp4/24000/25731m.mp4";
    public static string BarbellSquats = "https://videos.bodybuilding.com/video/mp4/54000/54671m.mp4";
    public static string DumbbellLunges = "https://videos.bodybuilding.com/video/mp4/54000/54851m.mp4";
    public static string RomanianDeadlift = "https://videos.bodybuilding.com/video/mp4/120000/120001m.mp4";
}
