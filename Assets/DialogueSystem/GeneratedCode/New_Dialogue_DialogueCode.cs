using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using DialogueSystem;
using System.Reflection;

namespace DialogueSystem.Code
{
    public class New_Dialogue_DialogueCode : IDialogueCode
    {
        private Dictionary<string, IDialogueCode.EventDelegate> eventFunctions = new Dictionary<string, IDialogueCode.EventDelegate>();
        private Dictionary<string, IDialogueCode.ConditionDelegate> conditionChecks = new Dictionary<string, IDialogueCode.ConditionDelegate>();
        private Dictionary<string, IDialogueCode.ConditionDelegate> dialogueChecks = new Dictionary<string, IDialogueCode.ConditionDelegate>();
        public Dictionary<string, IDialogueCode.EventDelegate> EventFunctions => eventFunctions;
        public Dictionary<string, IDialogueCode.ConditionDelegate> ConditionChecks => conditionChecks;
        public Dictionary<string, IDialogueCode.ConditionDelegate> DialogueChecks => dialogueChecks;
        public string GetVariable(string variableName) {
            return this.GetType().GetField(variableName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(this).ToString(); 
        }
        public New_Dialogue_DialogueCode() 
        {
            Start();
        }


        // Variables //
        // Variable(s) From Node: b30d63a8-6865-44dd-a010-fb4a7ebf602d //
int cats = 0;
string name = "Becky Sue";


        public void Start()
        {
            // Setup //
            eventFunctions.Add("New_Dialogue_538836df8f2f45c084d230196dcaf5e9",New_Dialogue_538836df8f2f45c084d230196dcaf5e9);
            eventFunctions.Add("New_Dialogue_30b2fb78ba854d0986b2329052b9ecc4",New_Dialogue_30b2fb78ba854d0986b2329052b9ecc4);
            eventFunctions.Add("New_Dialogue_46a607dd382c49d2a8c319de735e8157",New_Dialogue_46a607dd382c49d2a8c319de735e8157);
            dialogueChecks.Add("New_Dialogue_1105168d7cf24844bf025efcc454013f_9c8aec032182475b98a2d7ef96b5887c",New_Dialogue_1105168d7cf24844bf025efcc454013f_9c8aec032182475b98a2d7ef96b5887c);
            dialogueChecks.Add("New_Dialogue_1105168d7cf24844bf025efcc454013f_2e82160d18ce4a8b812ab874a3fbb742",New_Dialogue_1105168d7cf24844bf025efcc454013f_2e82160d18ce4a8b812ab874a3fbb742);
            dialogueChecks.Add("New_Dialogue_1105168d7cf24844bf025efcc454013f_37d9ac168f6f4307895dd5f569baf054",New_Dialogue_1105168d7cf24844bf025efcc454013f_37d9ac168f6f4307895dd5f569baf054);
            conditionChecks.Add("New_Dialogue_fd248e7fbb8b43a0bce1aab11f7ae84f",New_Dialogue_fd248e7fbb8b43a0bce1aab11f7ae84f);
            eventFunctions.Add("New_Dialogue_7b28363f37d44e84a6b37b83252ff565",New_Dialogue_7b28363f37d44e84a6b37b83252ff565);
            eventFunctions.Add("New_Dialogue_465747a1391f4eefac5d2a1344edd8e5",New_Dialogue_465747a1391f4eefac5d2a1344edd8e5);
            dialogueChecks.Add("New_Dialogue_27776fa84a1644d18680558c02f485c3_8c5a554f8fb74b0c88efa338653a0a9a",New_Dialogue_27776fa84a1644d18680558c02f485c3_8c5a554f8fb74b0c88efa338653a0a9a);
            dialogueChecks.Add("New_Dialogue_27776fa84a1644d18680558c02f485c3_342e6c8780eb44cb90168651300a2253",New_Dialogue_27776fa84a1644d18680558c02f485c3_342e6c8780eb44cb90168651300a2253);

        }

        // Dialogue Checks //
        // From Node: 1105168d-7cf2-4844-bf02-5efcc454013f //
        // Choice: 70,264 CATS NOW!!! - 9c8aec03-2182-475b-98a2-d7ef96b5887c //
        public bool New_Dialogue_1105168d7cf24844bf025efcc454013f_9c8aec032182475b98a2d7ef96b5887c()
        {
            return (true);
        }
        // From Node: 1105168d-7cf2-4844-bf02-5efcc454013f //
        // Choice: 2 Cats! - 2e82160d-18ce-4a8b-812a-b874a3fbb742 //
        public bool New_Dialogue_1105168d7cf24844bf025efcc454013f_2e82160d18ce4a8b812ab874a3fbb742()
        {
            return (true);
        }
        // From Node: 1105168d-7cf2-4844-bf02-5efcc454013f //
        // Choice: 1 cat - 37d9ac16-8f6f-4307-895d-d5f569baf054 //
        public bool New_Dialogue_1105168d7cf24844bf025efcc454013f_37d9ac168f6f4307895dd5f569baf054()
        {
            return (true);
        }
        // From Node: 27776fa8-4a16-44d1-8680-558c02f485c3 //
        // Choice: YES GIVE ME ALL THE CATS - 8c5a554f-8fb7-4b0c-88ef-a338653a0a9a //
        public bool New_Dialogue_27776fa84a1644d18680558c02f485c3_8c5a554f8fb74b0c88efa338653a0a9a()
        {
            return (cats > 1);
        }
        // From Node: 27776fa8-4a16-44d1-8680-558c02f485c3 //
        // Choice: No thank you! - 342e6c87-80eb-44cb-9016-8651300a2253 //
        public bool New_Dialogue_27776fa84a1644d18680558c02f485c3_342e6c8780eb44cb90168651300a2253()
        {
            return (cats <= 1);
        }


        // Condition Checks //
        // Condition From Node: fd248e7f-bb8b-43a0-bce1-aab11f7ae84f //
        public bool New_Dialogue_fd248e7fbb8b43a0bce1aab11f7ae84f() {
            return (cats > 5);
        }


        // Event Functions //
        // Event From Node: 538836df-8f2f-45c0-84d2-30196dcaf5e9 //
        public void New_Dialogue_538836df8f2f45c084d230196dcaf5e9() {
cats += 1;
        }
        // Event From Node: 30b2fb78-ba85-4d09-86b2-329052b9ecc4 //
        public void New_Dialogue_30b2fb78ba854d0986b2329052b9ecc4() {
cats += 2;
        }
        // Event From Node: 46a607dd-382c-49d2-a8c3-19de735e8157 //
        public void New_Dialogue_46a607dd382c49d2a8c319de735e8157() {
cats += 70264;
        }
        // Event From Node: 7b28363f-37d4-4e84-a6b3-7b83252ff565 //
        public void New_Dialogue_7b28363f37d44e84a6b37b83252ff565() {
cats -= 70000;
        }
        // Event From Node: 465747a1-391f-4eef-ac5d-2a1344edd8e5 //
        public void New_Dialogue_465747a1391f4eefac5d2a1344edd8e5() {
cats++;
        }

    }
}
