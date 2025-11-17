using DataVerseManager.Models;
using DataVerseManager.Services;
using DataVerseManager.UI;
using Spectre.Console;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DataVerseManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
          

            // LOW RANKERS
            Match.RunVisualMatch(Bulls, yourCoach.CoachTeam);
            SpectreGeneric.LoadScreen();
            // IMBALANCED MATCH
            Match.RunVisualMatch(myCoach.CoachTeam, Bulls);
            SpectreGeneric.LoadScreen();
            // MID RANKERS
            Match.RunVisualMatch(Heat, Warriors);
            SpectreGeneric.LoadScreen();
            // HIGH RANKERS
            Match.RunVisualMatch(Lakers, Knicks);
            SpectreGeneric.LoadScreen();
        }
    }
}

