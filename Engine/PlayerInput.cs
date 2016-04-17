using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Engine
{
    public class PlayerInput
    {
        public static void DoPlayerAction()
        {
            
        }

        //TODO - in the case of an item or bodypart with more than word, returns a string without spaces.
        /// <summary>
        /// If splitAction is of form 'equip item to bodypart', returns a list of strings containing two strings: item, then bodypart in list of strings. If splitArray is in the form 'equip item', will return a list containing only the string item.  Else returns null.
        /// </summary>
        /// <param name="splitAction"></param>
        /// <returns></returns>
        public static List<string> ProcessEquipCommand(string[] splitAction)
        {
            int count = 0;
            int positionOfWordTo = 0;

            List<string> itemBodyPart = new List<string>();

            string item = "";
            string bodyPart = "";

            foreach (string word in splitAction)
            {                
                if (word == "to")
                {
                    positionOfWordTo = count;
                }

                count += 1;
            }

            if (splitAction.Length <= 1)
            {
                return null;
            }

            if (positionOfWordTo == 0)
            {
                for (int i = 1; i < splitAction.Length - 1; i++)
                {
                    item += splitAction[i];
                }

                Array.Clear(splitAction, 0, splitAction.Length);

                itemBodyPart.Add(item);

                return itemBodyPart;
            }
            else
            {
                for (int i = 1; i < positionOfWordTo; i++)
                {
                    item += splitAction[i];
                }

                for (int i = positionOfWordTo + 1; i < splitAction.Length; i++)
                {
                    bodyPart += splitAction[i];
                }

                Array.Clear(splitAction, 0, splitAction.Length);

                itemBodyPart.Add(item);
                itemBodyPart.Add(bodyPart);

                return itemBodyPart;
            }
        }
    }
}