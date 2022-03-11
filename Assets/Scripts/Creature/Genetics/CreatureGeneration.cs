using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using UnityEditor;
using Utility;
using TextureConverter;
using Creature;

namespace Genetics
{
    public static class CreatureGeneration 
    {
        /// <summary>
        /// The chance of a mutation between 0-1 percent
        /// </summary>
        private const float MUTATION_CHANCE = 0.15f;
        private const string AnimationControllerLocation = "Animation/DemoController";

        private static string[] ImportantPartTypes =
            { "Bodies", "Ears", "Heads", "BackLegs", "FrontLegs" };
        private static string[] LesserPartTypes =
            { "Horns", "Tails" };
        private static string[] ManMade =
            { "Accessories", "Hats", "Masks" };

        /// <summary>
        /// Returns a randomly generated creature
        /// </summary>
        /// <returns></returns>
        public static GameObject CreateCreature(int? conversionSpeed = null) 
        {
            GeneticRepository genePool = Utility.Toolbox.Instance.GenePool;

            DNA dna = new DNA();
            foreach (string s in ImportantPartTypes) 
            {
                BodyPart bodypart = new BodyPart();
                if (!genePool.HasParts(s)) 
                {
                    Console.LogError($"No parts have been found in the {s} part folder! This is a required body part and creatures will look bad without it. Make sure you have parts in your parts folder.");
                    continue;
                }
                try
                {
                    bodypart = genePool.GetRandomPart(s).Value;
                    Pattern pattern = genePool.GetRandomPattern(bodypart);
                    PartHash part = new PartHash()
                    {
                        Category = s,
                        BodyPart = bodypart.Hash,
                        Pattern = pattern.Hash
                    };
                    dna.BodyPartHashs.Add(part);
                }
                catch (Exception e)
                {
                    Console.LogError($"Body Part {bodypart.Name} in {s} could not be initialized. This errored with this exception:\n{e}");
                    continue;
                }
            }

            foreach (string s in LesserPartTypes)
            {
                BodyPart bodypart = new BodyPart();
                if (!genePool.HasParts(s))
                {
                    Console.Log($"Body Part Repository, {s}, does not exist with parts in the StreamingAssets folder. Skipping...");
                    continue;
                }
                try
                {
                    bodypart = genePool.GetRandomPart(s).Value;
                    if (bodypart == null || UnityEngine.Random.Range(0f, 1f) < MUTATION_CHANCE)
                        continue;
                    PartHash part = new PartHash()
                    {
                        Category = s,
                        BodyPart = bodypart.Hash,
                        Pattern = genePool.GetRandomPattern(bodypart).Hash
                    };
                    dna.BodyPartHashs.Add(part);
                }
                catch (Exception e)
                {
                    Console.LogError($"Body Part {bodypart.Name} in {s} could not be initialized. This errored with this exception:\n{e}");
                    continue;
                }
            }

            foreach (string s in ManMade)
            {
                BodyPart bodypart = new BodyPart();
                if (!genePool.HasParts(s))
                {
                    Console.Log($"Body Part Repository, {s}, does not exist with parts in the StreamingAssets folder. Skipping...");
                    continue;
                }
                try
                {
                    bodypart = genePool.GetRandomPart(s).Value;
                    if (bodypart == null || UnityEngine.Random.Range(0f, 1f) < (1-MUTATION_CHANCE))
                        continue;
                    PartHash part = new PartHash()
                    {
                        Category = s,
                        BodyPart = bodypart.Hash,
                        Pattern = genePool.GetRandomPattern(bodypart).Hash
                    };
                    dna.BodyPartHashs.Add(part);
                }
                catch (Exception e)
                {
                    Console.LogError($"Body Part {bodypart.Name} in {s} could not be initialized. This errored with this exception:\n{e}");
                    continue;
                }
            }

            dna.Colors = RandomColors();

            return CreateCreature(dna, conversionSpeed);
        }

        /// <summary>
        /// Generates a random color and adds other potential pallets if "tis the season"
        /// </summary>
        /// <returns></returns>
        private static Color[] RandomColors() 
        {
            DateTime today = DateTime.Now;
            float ran = UnityEngine.Random.Range(0f, 1f);

            string colorPalette = RandomColorPicker.DefaultColorPalette;
            string usualLocation = "Data/ColorPalette/";

            System.Globalization.HebrewCalendar hc = new System.Globalization.HebrewCalendar();

            if (UnityEngine.Random.Range(0f, 1f) < MUTATION_CHANCE) 
            {
                List<string> potentialPalettes = new List<string>();

                if (hc.GetMonth(today) == 9 || hc.GetMonth(today) == 10)
                {
                    int day = 0;
                    if (hc.GetMonth(today) == 9)
                        day += hc.GetDayOfMonth(today);
                    if (hc.GetMonth(today) == 10)
                    {
                        day += hc.GetDaysInMonth(hc.GetYear(today), 9);
                        day += hc.GetDayOfMonth(today);
                    }

                    if (day >= 25-7 && day <= 33+2)
                        potentialPalettes.Add($"{usualLocation}hanukkah");
                }
                if (today.Month == 12)
                {
                    potentialPalettes.Add($"{usualLocation}xmas");
                    
                }
                if (today.Month == 10)
                {
                    potentialPalettes.Add($"{usualLocation}halloween");

                }
                if (today.Month == 12 || today.Month == 1) 
                {
                    int day = 0;
                    if (today.Month == 12)
                        day = today.Day;
                    if(today.Month == 1)
                        day = today.Day + DateTime.DaysInMonth(today.Year-1, 12);

                    if (day >= 26 - 7 && day <= 32 + 2)
                        potentialPalettes.Add($"{usualLocation}kwanzaa");
                }

                if (potentialPalettes.Count > 0)
                    colorPalette = potentialPalettes[UnityEngine.Random.Range(0, potentialPalettes.Count)];
            }

            Color[] colors = new Color[3];
            for (int i = 0; i < 3; i++)
                colors[i] = RandomColorPicker.RetriveRandomColor(RandomColorPicker.DefaultSeperationChar, colorPalette);

            return colors;
        }

        /// <summary>
        /// Generates a creature from a DNA strand
        /// </summary>
        /// <param name="dna"></param>
        /// <returns></returns>
        public static GameObject CreateCreature(DNA dna, int? conversionSpeed = null) 
        {
            List<GameObject> bodyParts = new List<GameObject>();
            
            foreach(PartHash part in dna.BodyPartHashs)
            {
                foreach(GameObject g in CreateBodyPart(part))
                    bodyParts.Add(g);
            }

            GameObject creature = new GameObject();
            GameObject offset = new GameObject();
            offset.name = "Offset";
            offset.transform.parent = creature.transform;
            offset.transform.position = new Vector3(0.097f, 0, -0.142f);
            offset.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

            GameObject mesh = ArmatureStitching.StitchObjects(bodyParts);
            mesh.name = "Mesh";
            mesh.transform.parent = offset.transform;
            mesh.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            creature.name = "Unnamed Creature";
            TextureController texCon = creature.AddComponent<TextureController>();
            texCon.colors = dna.Colors;
            if (conversionSpeed != null)
            { 
                texCon.CONVERSION_SPEED = conversionSpeed.Value;
            }

            // Add blinking
            creature.AddComponent<Creature.Face.Blink>();

            // Add Emotions
            creature.AddComponent<Creature.Emotions.EmotionCheck>();

            Animator a = mesh.AddComponent<Animator>();
            RuntimeAnimatorController rac = Resources.Load(AnimationControllerLocation) as RuntimeAnimatorController;
            a.runtimeAnimatorController = rac;

            CreatureAnimationControllerDemo cACD = creature.AddComponent<CreatureAnimationControllerDemo>();

            Face[] faceParts = creature.GetComponentsInChildren<Face>();
            if (faceParts.Length != 0) 
            {
                FaceTexture face = creature.AddComponent<FaceTexture>();
                foreach (Face f in faceParts) 
                {
                    if (f.IsMouth)
                    {
                        face.Mouth = f.gameObject;
                    }
                    else if (f.IsEyes) 
                    {
                        face.Eyes = f.gameObject;
                    }
                    GameObject.Destroy(f);
                }
            }

            FeetSound[] feet = creature.GetComponentsInChildren<FeetSound>();
            string frontFeetSound = string.Empty, backFeetSound = string.Empty;
            foreach (FeetSound foot in feet)
            {
                if (!string.IsNullOrWhiteSpace(foot.Sound)) 
                {
                    if (foot.IsFrontFeet)
                    {
                        frontFeetSound = foot.Sound;
                    }
                    else
                    {
                        backFeetSound = foot.Sound;
                    }
                }
                FeetSound.Destroy(foot);
            }

            BoneToPick[] boneModi = creature.GetComponentsInChildren<BoneToPick>();
            foreach (BoneToPick bone in boneModi) 
            {
                if (!string.IsNullOrWhiteSpace(bone.BoneOffset))
                {
                    Transform pick = ArmatureStitching.FindChildByName(bone.BoneOffset, creature.transform);
                    pick.position += bone.Offset;
                }
                BoneToPick.Destroy(bone);
            }

            // Creature interacts with grass shader
            creature.AddComponent<ShaderInteractor>();

            CreatureController c = creature.AddComponent<CreatureController>();
            c.SetUp(dna, a, Guid.NewGuid().ToString(), frontFeetSound, backFeetSound);

            SphereCollider sphere = creature.AddComponent<SphereCollider>();
            sphere.center = new Vector3(0, 0.3f, 0);
            sphere.radius = 0.3f;

            return creature;
        }

        /// <summary>
        /// Breeds two parents and returns the resulting DNA
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public static DNA Breed(DNA parent1, DNA parent2)
        {
            /* To generate a child, take an attribute from each catagory and randomly pick between the two
             * Have a small % chance for a 'mutation' which will pick a random attribute
             */
            throw new NotImplementedException();
        }

        private static List<GameObject> CreateBodyPart(PartHash partBits) 
        {
            List<GameObject> g = new List<GameObject>();
            GeneticRepository genePool = Utility.Toolbox.Instance.GenePool;
            BodyPart bodyPart = genePool.GetBodyPart(partBits.BodyPart);
            Pattern pattern = genePool.GetPattern(partBits.Pattern);

            GameObject partObject = Siccity.GLTFUtility.Importer.LoadFromFile(bodyPart.FileLocation);
            partObject.name = "TempObj";
            partObject.transform.localScale = Vector3.one * bodyPart.Scale;

            Texture2D texture2D = new Texture2D(1,1);
            byte[] textureBytes = System.IO.File.ReadAllBytes(pattern.FileLocation);
            Renderer[] models = partObject.transform.GetComponentsInChildren<Renderer>();
            if (models.Length == 0)
                Console.LogError($"Body part {bodyPart.Name} in {bodyPart.Type} had no skinned meshes. This body part will not render correctly. Please make sure your model is correct.");

            bool useTexture = ImageConversion.LoadImage(texture2D, textureBytes, false);

            int i = 0;
            foreach (Renderer renderer in models) 
            {
                if (i == 0) 
                {
                    if (!string.IsNullOrWhiteSpace(bodyPart.OffsetBone) && bodyPart.Offset != null) 
                        renderer.gameObject.AddComponent<BoneToPick>().Populate(bodyPart.OffsetBone, bodyPart.Offset);
                    if (bodyPart.Type == "FrontLegs" || bodyPart.Type == "BackLegs")
                        renderer.gameObject.AddComponent<FeetSound>().Populate(bodyPart.Sound, bodyPart.Type == "FrontLegs");
                }
                i++;
                if (useTexture && texture2D.width != 1)
                {
                    renderer.gameObject.AddComponent<MaterialConversion>()
                        .SetMainTexture(texture2D, partBits.Pattern);
                } 
                else 
                {
                    Console.LogWarning($"Pattern Texture {pattern.Name} could not be loaded into the material. The load result is {useTexture}, and the 2D Texture size is ({texture2D.width}, {texture2D.height}).");
                }
                foreach (Material m in renderer.materials)
                    m.shader = genePool.GetShader(bodyPart.Shader);
            }

            if (!string.IsNullOrWhiteSpace(bodyPart.Eyes))
            {
                BodyPart eyes = genePool.GetBodyPartByName(bodyPart.Eyes);
                if (eyes != null)
                {
                    GameObject eyesObject = Siccity.GLTFUtility.Importer.LoadFromFile(eyes.FileLocation);
                    g.Add(eyesObject);
                    Renderer[] renderers = eyesObject.GetComponentsInChildren<Renderer>();
                    if(renderers.Length > 0)
                        renderers[0].gameObject.AddComponent<Face>().Populate(true, false);
                }
            }

            if (!string.IsNullOrWhiteSpace(bodyPart.Mouth))
            {
                BodyPart mouth = genePool.GetBodyPartByName(bodyPart.Mouth);
                if (mouth != null)
                {
                    GameObject mouthObject = Siccity.GLTFUtility.Importer.LoadFromFile(mouth.FileLocation);
                    g.Add(mouthObject);
                    Renderer[] renderers = mouthObject.GetComponentsInChildren<Renderer>();
                    if (renderers.Length > 0)
                        renderers[0].gameObject.AddComponent<Face>().Populate(false, true);
                }
            }

            g.Add(partObject);
            return g;
        }
    }
}