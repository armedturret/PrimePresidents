﻿using UMM;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System;
using System.Collections.Generic;

namespace PrimePresidents
{
    [UKPlugin("gov.PrimePresidents","Prime Presidents", "1.0.0", "Jowari da", true, true)]
    public class Presidents : UKMod
    {
        private static Harmony harmony;

        private static readonly string BaseDirectory = Directory.GetParent(Application.dataPath).FullName;
        internal static readonly AssetBundle PresidentsAssetBundle = AssetBundle.LoadFromFile(Path.Combine(BaseDirectory, "BepInEx/UMM Mods/PrimePresidents/Assets/primepresidents"));

        public override void OnModLoaded()
        {
            Debug.Log("Prime presidents starting");

            //start harmonylib to swap assets
            harmony = new Harmony("gov.PrimePresidents");
            harmony.PatchAll();
        }

        public override void OnModUnload()
        {
            harmony.UnpatchSelf();
            base.OnModUnload();
        }

        private static SubtitledAudioSource.SubtitleDataLine MakeLine(string subtitle, float time){
            var sub = new SubtitledAudioSource.SubtitleDataLine();
            sub.subtitle = subtitle;
            sub.time = time;
            return sub;
        }

        //replace minos prime data
        [HarmonyPatch(typeof(MinosPrime), "Start")]
        internal class Patch01
        {
            static void Postfix(MinosPrime __instance){
                Debug.Log("Replacing minos voice lines");

                //set judgement to biden blast
                AudioClip[] dropkickLines = new AudioClip[1];
                dropkickLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_blast.mp3");
                __instance.dropkickVoice = dropkickLines;

                //set ppt to choco chip
                AudioClip[] comboLines = new AudioClip[1];
                comboLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_chocolate_chocolate.mp3");
                __instance.comboVoice = comboLines;

                //set thy end is now to come here bucko
                AudioClip[] boxingLines = new AudioClip[1];
                boxingLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_come_here.mp3");
                __instance.boxingVoice = boxingLines;

                //set die to jowarida
                AudioClip[] riderkickLines = new AudioClip[1];
                riderkickLines[0] = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_joewareeda.mp3");
                __instance.riderKickVoice = riderkickLines;

                //set weak to thats it
                __instance.phaseChangeVoice = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_thats_it.mp3");

                Debug.Log("Replacing minos mesh texture");
                               
                //set texture to be biden prime
                var body = __instance.transform.Find("Model").Find("MinosPrime_Body.001");
                var renderer = body.GetComponent<Renderer>();
                var newMat = new Material(renderer.material);
                newMat.mainTexture = PresidentsAssetBundle.LoadAsset<Texture2D>("JoePrime_1.png");
                renderer.sharedMaterial = newMat;
            }
        }

        //replace captions for minos attacks
        [HarmonyPatch(typeof(SubtitleController), nameof(SubtitleController.DisplaySubtitle), new Type[]{typeof(string), typeof(AudioSource)})]
        internal class Patch02
        {
            static void Prefix(ref string caption, AudioSource audioSource){
                //change caption
                if(caption == "Thy end is now!")
                {
                    caption = "Come here, bucko!";
                }
                else if(caption == "Die!"){
                    caption = "Jowarida!";
                }
                else if(caption == "WEAK"){
                    caption = "THAT'S IT BUD";
                }
                else if(caption == "Judgement!"){
                    caption = "Biden blast!";
                }
                else if(caption == "Crush!"){
                    caption = "[REPLACE ME]";
                }
                else if(caption == "Prepare thyself!"){
                    caption = "Eat some chocolate chocolate chip!";
                }
            }
        }


        //use map info to inject data
        [HarmonyPatch(typeof(StockMapInfo), "Awake")]
        internal class Patch03
        {
            static void Postfix(StockMapInfo __instance){
                //try to find dialog in scene and replace it
                foreach(var source in Resources.FindObjectsOfTypeAll<AudioSource>())
                {
                    if(source.clip){
                        bool replaced = false;
                        var subtitles = new List<SubtitledAudioSource.SubtitleDataLine>();
                        if(source.clip.GetName() == "mp_intro2")
                        {
                            Debug.Log("Replacing intro");
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_intro.mp3");
                            replaced = true;

                            subtitles.Add(MakeLine("Ahh...", 0f));
                            subtitles.Add(MakeLine("Gotta give it to our penitentiaries", 1.25f));
                            subtitles.Add(MakeLine("they do a good job of keeping a man locked up", 3.15f));
                            subtitles.Add(MakeLine("Donald Trump, now you uh...", 6.4f));
                            subtitles.Add(MakeLine("Listen up, ok?", 8.75f));
                            subtitles.Add(MakeLine("The 2024 election is right around the corner", 10.4f));
                            subtitles.Add(MakeLine("The American people want change, they want progress not you", 13f));
                            subtitles.Add(MakeLine("Your only legacy will be a smear on the history of this great nation", 16.7f));
                            subtitles.Add(MakeLine("Uhh... listen fa-, listen machine...", 21.25f));
                            subtitles.Add(MakeLine("Now I do gotta thank you for my freedom, it's the basis of which our country was formed", 24.3f));
                            subtitles.Add(MakeLine("So your patriotism, I uh...", 29.35f));
                            subtitles.Add(MakeLine("I do appreciate it", 30.9f));
                            subtitles.Add(MakeLine("but the uh...", 32.75f));
                            subtitles.Add(MakeLine("the crimes that you have committed against America and its people... ", 34.4f));
                            subtitles.Add(MakeLine("They uh...", 38f));
                            subtitles.Add(MakeLine("They've not been forgotten alright?", 38.9f));
                            subtitles.Add(MakeLine("And your punishment, according to the constitution is uh...", 40.34f));
                            subtitles.Add(MakeLine("I... it's DEATH", 43.75f));
                        }
                        else if(source.clip.GetName() == "mp_outro")
                        {
                            Debug.Log("Replacing outro");
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_outro.mp3");
                            replaced = true;

                            subtitles.Add(MakeLine("Aagh!", 0f));
                            subtitles.Add(MakeLine("It's Joever", 4f));
                            subtitles.Add(MakeLine("Oh hey a cool robot", 5f));
                            subtitles.Add(MakeLine("Ah, American made, just the way I like to see it", 6.8f));
                            subtitles.Add(MakeLine("American machines like YOU", 9.75f));
                            subtitles.Add(MakeLine("are what really drives this great nation", 11.6f));
                            subtitles.Add(MakeLine("keep up the good work, you're doing America proud", 13.7f));
                            subtitles.Add(MakeLine("Anyway, I uh...", 16.2f));
                            subtitles.Add(MakeLine("I forgot how to breathe so I gotta go", 17.36f));
                        }
                        else if(source.clip.GetName() == "mp_deathscream")
                        {
                            Debug.Log("Replacing death scream");
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_soda.mp3");
                            replaced = true;

                            subtitles.Add(MakeLine("SODA!", 0f));
                        }
                        else if(source.clip.GetName() == "mp_useless")
                        {
                            Debug.Log("Replacing useless");
                            source.clip = PresidentsAssetBundle.LoadAsset<AudioClip>("biden_nicetry.mp3");
                            replaced = true;

                            subtitles.Add(MakeLine("Nice try, kid", 0f));
                        }

                        //update subtitles if needed
                        if(replaced){
                            var subsource = source.GetComponent<SubtitledAudioSource>();
                            Traverse field = Traverse.Create(subsource).Field("subtitles");
                            (field.GetValue() as SubtitledAudioSource.SubtitleData).lines = subtitles.ToArray();
                        }
                    }
                }

                //replace minos meshes
                foreach(var renderer in Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>())
                {
                    if(renderer.gameObject.name == "MinosPrime_Body.001")
                    {
                        var newMat = new Material(renderer.material);
                        newMat.mainTexture = PresidentsAssetBundle.LoadAsset<Texture2D>("JoePrime_1.png");
                        renderer.sharedMaterial = newMat;
                    }
                }
            }
        }

        //replace boss names
        [HarmonyPatch(typeof(BossHealthBar), "Awake")]
        internal class Patch04
        {
            static void Prefix(BossHealthBar __instance)
            {
                if(__instance.bossName == "MINOS PRIME"){
                    __instance.bossName = "BIDEN PRIME";
                }
                if(__instance.bossName == "SISYPHUS PRIME"){
                    __instance.bossName = "TRUMP PRIME";
                }
                if(__instance.bossName == "FLESH PANOPTICON"){
                    __instance.bossName = "OBAMANOPTICON";
                }
            }
        }

        //replace intro texts
        [HarmonyPatch(typeof(LevelNamePopup), "Start")]
        internal class Patch05
        {
            //replace name AFTER to not interfere with saves
            static void Postfix(LevelNamePopup __instance)
            {
                Traverse field = Traverse.Create(__instance).Field("nameString");
                if(field.GetValue() as string == "SOUL SURVIVOR"){
                    field.SetValue("CHIEF OF STATE");
                }
                if(field.GetValue() as string == "WAIT OF THE WORLD"){
                    field.SetValue("SIN OF THE APPRENTICE");
                }
            }
        }
    }
}
