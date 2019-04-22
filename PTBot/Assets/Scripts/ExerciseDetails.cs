﻿public class ExerciseDetails
{
    private string[][] Exercises = new string[][]
    {
        // { name, url, description}
        // Arm Exercises
       new string[] { " Alternate Dumbbell Curls", "https://videos.bodybuilding.com/video/mp4/28000/28901m.mp4", "1. Stand up straight with a dumbbell in each hand. Keep upper arms stationary.\n2. Exhale and curl left hand dumbbell while contracting your left bicep. Rotate the dumbbell during this motion so your palms are facing towards your upper body.\n3. When fully contracted squeeze your bicep, then lower the weight back down to the starting position as you inhale.\n4. Repeat with right arm.\n5. Continue alternating arms for desired amount of repetitions.\n"},
       new string[] { " Alternate Dumbbell Curls Incline", "https://videos.bodybuilding.com/video/mp4/24000/25601m.mp4", "1. Sit down on incline bench with dumbbells held in each hand at arm’s length.\n2. Exhale and curl left hand dumbbell while contracting your left bicep.\n3. When fully contracted squeeze your bicep, then lower the weight back down to the starting position as you inhale.\n4. Repeat with right arm.\n5. Continue alternating arms for desired amount of repetitions.\n"},
       new string[] { " Barbell Curls", "https://videos.bodybuilding.com/video/mp4/24000/25681m.mp4", "1. Stand upright and hold the barbell at a shoulder-width grip.\n2. Hold upper arms stationary, curl the barbell and contract whilst exhaling.\n3. Squeeze the biceps when contracted.\n4. Inhale whilst lowering the barbell to the starting position.\n5. Repeat for desired amount of repetitions.\n"},
       new string[] { " Barbell Curls Incline", "https://videos.bodybuilding.com/video/mp4/24000/25691m.mp4", "1. Lie against an incline bench holding the barbell hanging down.\n2. Hold upper arms stationary, curl the barbell and contract whilst exhaling.\n3. Squeeze the biceps when contracted.\n4. Inhale whilst lowering the barbell to the starting position.\n5. Repeat for desired number of repetitions.\n"},
       new string[] { " Concentration Curls", "https://videos.bodybuilding.com/video/mp4/32000/32091m.mp4", "1. Sit on a flat bench and hold a dumbbell between your legs.\n2. Use your left arm to pick the dumbbell up and place your upper left arm on your inner left thigh. The palm of your left hand should face away from your left thigh.\n3. Hold the upper arm stationary and curl the dumbbell whilst exhaling and contracting the bicep.\n4. Squeeze the bicep when contracted.\n5. Lower the dumbbell to the starting position as you inhale.\n6. Repeat for desired number of repetitions, then repeat with your right arm.\n"},
       new string[] { " Hammer Curls", "https://videos.bodybuilding.com/video/mp4/28000/29371m.mp4", "1. Stand up straight with a dumbbell in each hand. Keep upper arms stationary. The palms of your hands should face your torso.\n2. Exhale and curl left hand dumbbell while contracting your left bicep.\n3. When fully contracted squeeze your bicep, then lower the weight back down to the starting position as you inhale.\n4. Repeat with right arm.\n5. Continue alternating arms for desired amount of repetitions.\n"},
       new string[] { " Tricep Pushdowns", "https://videos.bodybuilding.com/video/mp4/32000/32901m.mp4", "1. Attach either a straight / angled bar or a rope to a high cable pulley machine.\n2. Grab the bar with your palms facing down, and stand slightly leaning forward.\n3. Exhale whilst bringing the bar down by contracting your triceps until your arms are fully extended perpendicular to the floor.\n4. Hold the bar whilst fully contracted and slowly bring up the bar and inhale during this motion.\n5. Repeat for desired amount of repetitions.\n"},
       new string[] { " Tricep Dips", "https://videos.bodybuilding.com/video/mp4/54000/54741m.mp4", "1. Place a bench behind your back.\n2. Face away from the bench and hold onto the bench your hands fully extended and separated at shoulder width.\n3. Extend your legs forward perpendicular to your torso.\n4. Slowly inhale and lower your body.\n5. Contract your triceps to lift yourself up again.\n6. Repeat for desired amount of repetitions.\n"},
       // Back Exercises
       new string[] { " Barbell Shrug", "https://videos.bodybuilding.com/video/mp4/24000/25761m.mp4", "1. Stand upright and hold the barbell with both hands and your palms facing your thighs.\n2. Exhale and lift the barbell with only your shoulders. The biceps shouldn’t help lift the barbell for this exercise and your arms should remain extended at all times.\n3. Lower the barbell back to the starting position.\n4. Repeat for desired number of repetitions.\n"},
       new string[] { " Dumbbell Shrug", "https://videos.bodybuilding.com/video/mp4/28000/29131m.mp4", "1. Stand with a dumbbell in each hand with your palms facing your torso.\n2. Exhale and lift the dumbbells with only your shoulders. The biceps shouldn’t help lift the dumbbells for this exercise and your arms should remain extended at all times.\n3. Lower the dumbbells back to the starting position.\n4. Repeat for desired number of repetitions.\n"},
       new string[] { " Lat Pulldowns", "https://videos.bodybuilding.com/video/mp4/32000/32081m.mp4", "1. Sit down on a pull-down machine and grab the bar with your palms facing forward. You can either use a close, medium or wide grip.\n2. Bring your torso back about 30-degrees and stick your chest out.\n3. Exhale and bring the bar down until it touches your upper chest. Squeeze the back muscles when fully contracted.\n4. Inhale and slowly raise the bar back to the starting position.\n5. Repeat for desired number of repetitions."},
       new string[] { " Pullups", "https://videos.bodybuilding.com/video/mp4/30000/30171m.mp4", "1. Grab the pull-up bar with your palms facing forward. You can either use a wide-grip, close-grip or medium-grip.\n2. Pull your torso up until the bar reaches your upper chest. Make sure to exhale during this step. Squeeze the back muscles when fully contracted. Only your arms should move.\n3. Start to inhale and slowly lower your torso to the starting position.\n4. Repeat for desired number of repetitions."},
       new string[] { " Seated Cable Rows", "https://videos.bodybuilding.com/video/mp4/30000/30431m.mp4", "1. Sit down on a low-pulley machine and make sure your knees are slightly bent.\n2. Grab the bar handles and pull back until your torso is at a 90-degree angle from your legs. You should slightly arch your back and stick your chest out.\n3. Pull the handles back to your torso and exhale during this step.\n4. Slowly inhale and return to the starting position.\n5. Repeat for desired number of repetitions."},
      // Chest Exercises
       new string[] { " Barbell Bench Press", "https://videos.bodybuilding.com/video/mp4/54000/54651m.mp4", "1. Lie down on a flat bench. You can use a close, medium or wide grip on the barbell.\n2. Lift the bar above your body with your arms locked.\n3. Inhale and slowly lower the bar until it touches your middle chest.\n4. Exhale and push the bar back to the starting position.\n5. Repeat for desired number of repetitions."},
       new string[] { " Dumbbell Bench Press Decline", "https://videos.bodybuilding.com/video/mp4/32000/32131m.mp4", "1. Secure your legs at the end of the decline bench and lie down with a dumbbell on each hand on top of your thighs. The palms of your hands should face each other.\n2. Move the dumbbells in front of you at shoulder width.\n3. Once at shoulder width rotate your wrists forward so that the palms of your hands are facing away from you.\n4. Exhale and lower the weights slowly to your side.\n5. Push the dumbbells up using your pectoral muscles until your arms are locked.\n6. Hold then slowly lower the weights.\n7. Repeat for desired number of repetitions."},
       new string[] { " Dumbbell Bench Press", "https://videos.bodybuilding.com/video/mp4/28000/28871m.mp4", "1. Lie down on a flat bench with a dumbbell in each hand resting on the top of your thighs. The palms of your hands should face each other.\n2. Lift the dumbbells so you can hold them in front of you at shoulder width.\n3. Once at shoulder width rotate your wrists forward so your palms face away from you and the dumbbells should be at the sides of your chest.\n4. Exhale and use your chest to push the dumbbells up, lock your arms at the top of the lift and squeeze your chest.\n5. Slowly lower the dumbbells back to the starting position.\n6. Repeat for desired number of repetitions."},
       new string[] { " Dumbbell Flyes", "https://videos.bodybuilding.com/video/mp4/28000/28921m.mp4", "1. Lie down on a flat bench with a dumbbell in each hand at shoulder width. The palms of your hand should face each other. Raise the dumbbells up.\n2. Inhale and lower your arms out at both sides in a wide arc.\n3. Exhale and lift the dumbbells back to the starting position.\n4. Hold and repeat for desired number of repetitions."},
       new string[] { " Pushups", "https://videos.bodybuilding.com/video/mp4/30000/30191m.mp4", "1. Lie on the floor face down and hold your torso up at arm’s length.\n2. Inhale and lower yourself downward until your chest almost touches the floor.\n3. Exhale and press your upper body back to the starting position.\n4. Repeat for desired number of repetitions."},
       // Core Exercises
       new string[] { " Air Bikes", "https://videos.bodybuilding.com/video/mp4/24000/25571m.mp4", "1. Lie flat on the floor with your lower back pressed to the ground. Put your hands beside your head. Lift your shoulders into the crunch position.\n2. Lift your knees perpendicular to the floor.\n3. Go through a cycle pedal motion by kicking forward with your left leg and bring your right leg and your left elbow together. Make sure to exhale during this step.\n4. Inhale and return to the starting position, then crunch to the opposite side.\n5. Repeat for desired number of repetitions."},
       new string[] { " Barbell Side Bends", "https://videos.bodybuilding.com/video/mp4/24000/25781m.mp4", "1. Stand upright while holding a barbell on the back of your shoulders slightly below the neck.\n2.  Keep your back straight and head up, and bend at the waist to the right as far as possible. Make sure to inhale during this motion.\n3. Exhale and return to the starting position.\n4. Now bend to the left as far as possible, then return to the starting position.\n5. Repeat for desired number of repetitions."},
       new string[] { " Dumbbell Side Bends", "https://videos.bodybuilding.com/video/mp4/28000/29141m.mp4", "1. Stand upright with a dumbbell in your left hand and palm facing your torso.\n2. Keep your back straight and head up, and bend at the waist to the right as far as possible. Make sure the inhale during this motion.\n3. Exhale and return to the starting position.\n4. Now bend to the left as far as possible, then return to the starting position.\n5. Repeat for desired number of repetitions then repeat the exercise by holding the dumbbell in your right hand instead."},
       new string[] { " Hanging Leg Raises", "https://videos.bodybuilding.com/video/mp4/28000/29401m.mp4", "1. Hang from a chin-up bar with both arms extended at arm’s length.\n2. Raise your legs until the torso makes a 90-degree angle with your legs. Make sure to exhale during this motion.\n3. Inhale and return to the starting position slowly.\n4. Repeat for the desired number of repetitions."},
       new string[] { " Russian Twists", "https://videos.bodybuilding.com/video/mp4/30000/30361m.mp4", "1. Lie down on the floor and bend your legs at the knees.\n2. Lift your upper body to create a V-shape with your thighs and fully extend your arms in front of you perpendicular to your torso.\n3. Twist your torso to the left side until your arms are parallel with the floor and exhale during this motion.\n4. Move back to the starting position and exhale during this motion.\n5. Repeat for desired number of repetitions."},
       new string[] { " Seated Barbell Twists", "https://videos.bodybuilding.com/video/mp4/30000/30391m.mp4", "1. Sit at the end of a flat bench with the barbell behind your head resting on the base of your neck.\n2. Keep your feet and head stationary and move your waist from side to side as far as you feel comfortable and exhale.\n3. Exhale and return to the starting position.\n4. Repeat for desired number of repetitions."},
       // Leg Exercises
       new string[] { " Barbell Deadlift", "https://videos.bodybuilding.com/video/mp4/118000/118911m.mp4", "1. Centre the bar over your feet and keep your feet hip-width apart.\n2. You can use an alternate grip and bend to grip the bar.\n3. Inhale and lower your hips and flex your knees, look forward with your head.\n4. Keep your chest up and your back arched. Begin driving through your heels to move the weight forward.\n5. When the bar passes your knees, pull the bar back and pull your shoulder blades together.\n6. Bend your hips to lower the bar to the floor.\n7. Repeat for desired number of repetitions."},
       new string[] { " Barbell Lunges", "https://videos.bodybuilding.com/video/mp4/24000/25731m.mp4", "1. You can use a squat rack if you like.\n2. Place the bar on the back of your shoulders (slightly below the neck).\n3. Hold the bar using both arms at each side, step forward with your left leg and squat through your hips. Make sure to inhale during this step.\n4. Keep your knee behind your toes during this movement, and keep your torso upright.\n5. Use the heel of your foot to push up and go back to the starting position. Make sure you exhale during this step.\n6. Repeat for desired number of repetitions whilst alternating with your right leg."},
       new string[] { " Barbell Squats", "https://videos.bodybuilding.com/video/mp4/54000/54671m.mp4", "1. You can use a squat rack if you like.\n2. Place the bar on the back of your shoulders (slightly below the neck).\n3. Keep your head up and maintain a straight back.\n4. Slowly lower the bar by bending your knees and sit back with your hips.\n5. Keep your head up and continue lowering until your hamstrings are on your calves as you inhale.\n6. Exhale and push the floor with your foot and straighten your legs to raise the bar back to the starting position.\n7. Repeat for desired number of repetitions."},
       new string[] { " Dumbbell Lunges", "https://videos.bodybuilding.com/video/mp4/54000/54851m.mp4", "1. Stand upright and hold a dumbbell in each hand by your sides.\n2. Step forward with your left leg and lower your upper body down. Keep your knee behind your toes. Make sure to inhale during this step.\n3. Use the heel of your foot to push up and return to the starting position. Make sure to exhale during this step.\n4. Repeat for desired number of repetitions whilst alternating with your right leg."},
       new string[] { " Romanian Deadlift", "https://videos.bodybuilding.com/video/mp4/120000/120001m.mp4", "1. Hold the bar at hip level with your palms facing down. Keep your back arched and slightly bend your knees.\n2. Lower the bar by moving your bum back as far as you can. Keep looking forward and keep the bar close to your body.\n3. Drive your hips forward to stand up tall.\n4. Repeat for desired number of repetitions." }
    };

    public string[][] GetEntireArray()
    {
        return Exercises;
    }

    public int GetArrayIndex(string value)
    {
        for (int i = 0; i < Exercises.GetLength(0); i++)
        {
            if (Exercises[i][0] == value) { return i; }
        }
        return -1;
    }

    public string GetArrayValue(int firstIndex, int secondIndex) { return Exercises[firstIndex][secondIndex]; }
}
