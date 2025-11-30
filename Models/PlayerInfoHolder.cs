using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVerseManager.Models
{
    public static class PlayerInfoHolder
    {
        public static List<string> FirstNames = new List<string> {
    "Aaron","Adrian","Alec","Alex","Andre","Anthony","Antonio","Ashton","Avery","Barry",
    "Ben","Blake","Brandon","Bruce","Bryan","Caleb","Cameron","Carl","Cedric","Charles",
    "Chris","Clarence","Cody","Cole","Colin","Curtis","Dale","Damon","Daniel","Darren",
    "David","Derek","Devin","Dominic","Don","Douglas","Dylan","Eddie","Elijah","Elliot",
    "Eric","Ethan","Evan","Felix","Floyd","Gabe","Gary","Gavin","Grant","Greg",
    "Harold","Henry","Hunter","Isaiah","Ivan","Jack","Jackson","Jacob","Jalen","Jared",
    "Jason","Jeff","Jeremiah","Jerome","Jesse","Joel","John","Jonah","Jordan","Joseph",
    "Juan","Julian","Justin","Keith","Kenneth","Kevin","Kobe","Kyle","Lance","Larry",
    "Leonard","Liam","Logan","Louis","Malcolm","Marcus","Mario","Mark","Mason","Matthew",
    "Maurice","Max","Michael","Miles","Nate","Nathan","Noah","Oscar","Patrick","Paul"
    };
        public static List<string> Nicknames = new List<string> {
    "“Ace”","“Alpha”","“Anvil”","“Atlas”","“Bam”","“Beast”","“Blaze”","“Bolt”","“Boomer”","“Boss”",
    "“Bullet”","“Cannon”","“Chief”","“Clutch”","“Commander”","“Crusher”","“Dagger”","“Dash”","“Diesel”","“Dino”",
    "“Dragon”","“Duke”","“Eagle”","“Edge”","“Enforcer”","“Falcon”","“Fearless”","“Flash”","“Giant”","“Gladiator”",
    "“Goliath”","“Hammer”","“Hawk”","“Hurricane”","“Ice”","“Iron”","“Jet”","“Judge”","“King”","“Knockout”",
    "“Legend”","“Lightning”","“Lion”","“Machine”","“Mammoth”","“Maverick”","“Miracle”","“Monster”","“Nightmare”","“Nova”",
    "“Omega”","“Panther”","“Phantom”","“Phoenix”","“Predator”","“Prime”","“Prodigy”","“Razor”","“Reaper”","“Rhino”",
    "“Rocket”","“Rogue”","“Samurai”","“Savage”","“Shadow”","“Shark”","“Shockwave”","“Slayer”","“Sniper”","“Specter”",
    "“Speedster”","“Steel”","“Storm”","“Tank”","“Thunder”","“Titan”","“Turbo”","“Vandal”","“Viper”","“Warrior”",
    "“Whirlwind”","“Wildcat”","“Wolf”","“Vortex”","“Voltage”","“Warden”","“Reactor”","“Magnet”","“Cobra”","“Domino”",
    "“Rumble”","“Vector”","“Wizard”","“Ranger”","“Nomad”","“Oracle”","“Pilot”","“Raptor”","“Blizzard”","“Patriot”"
};
        public static List<string> LastNames = new List<string> {
    "Adams","Andrews","Armstrong","Bailey","Barnes","Barrett","Bell","Bennett","Bishop","Black",
    "Boyd","Bradley","Brewer","Brooks","Brown","Bryant","Burke","Butler","Cain","Campbell",
    "Carlson","Carpenter","Carter","Chambers","Chapman","Clark","Coleman","Collins","Cooper","Cox",
    "Crawford","Cruz","Cunningham","Curtis","Daniels","Dawson","Day","Dean","Dixon","Douglas",
    "Dunn","Edwards","Elliott","Ellis","Evans","Ferguson","Fernandez","Fields","Fisher","Fleming",
    "Ford","Foster","Fowler","Fox","Franklin","Freeman","Fuller","Garcia","Gardner","George",
    "Gibson","Gilbert","Gomez","Graham","Grant","Gray","Green","Griffin","Guerrero","Hall",
    "Hamilton","Hansen","Harper","Harris","Harrison","Hart","Hayes","Henderson","Henry","Holmes",
    "Howard","Hudson","Hughes","Hunt","Jackson","Jacobs","James","Jefferson","Jenkins","Jensen",
    "Johnson","Jones","Jordan","Keller","Kelly","Kennedy","Kim","King","Knight","Larson"
};
        public static List<string> Countries = new List<string> {
    "USA","Canada","Mexico","Brazil","Argentina","Chile","Uruguay","Paraguay","Colombia","Venezuela",
    "Peru","Bolivia","Dominican Republic","Puerto Rico","Cuba","Jamaica","Bahamas","Barbados","Trinidad & Tobago","Costa Rica",
    "Panama","Honduras","Guatemala","El Salvador","Nicaragua","Haiti","France","Spain","Germany","Italy",
    "Greece","United Kingdom","Turkey","Serbia","Croatia","Slovenia","Bosnia & Herzegovina","Montenegro","Lithuania","Latvia",
    "Estonia","Poland","Czech Republic","Slovakia","Hungary","Romania","Bulgaria","Russia","Ukraine","Finland",
    "Sweden","Norway","Denmark","Iceland","Portugal","Belgium","Netherlands","Switzerland","Austria","Australia",
    "New Zealand","China","Japan","South Korea","Philippines","Indonesia","Malaysia","Thailand","Vietnam","Singapore",
    "Taiwan","India","Pakistan","Iran","Iraq","Saudi Arabia","Qatar","United Arab Emirates","Kuwait","Oman",
    "Jordan","Lebanon","Syria","Israel","Egypt","Morocco","Algeria","Tunisia","Nigeria","Ghana",
    "South Africa","Senegal","Ivory Coast","Cameroon","Angola","Kenya","Uganda","Tanzania","Zimbabwe","Rwanda",
    "Mozambique","Zambia","Madagascar","Ethiopia","Sudan","South Sudan","Kazakhstan","Georgia","Armenia","Azerbaijan",
    "Turkey","Belarus","Luxembourg","Cyprus","Malta","Moldova","Kosovo","Macedonia","Sri Lanka","Bangladesh"
};

        public static List<string> imagePath = new List<string>
        {
            "images/default.png",
            "images/p1.png",
            "images/p2.png",
            "images/p3.png",
            "images/p4.png",
            "images/p5.png",
            "images/p6.png",
            "images/p7.png",
            "images/p8.png",
            "images/p9.png",
            "images/p10.png",
            "images/p11.png",
            "images/p12.png"
        };

       public static List<string> infoDump = new List<string>()
{
    "Grew up shooting hoops on a cracked driveway.\nWatched old NBA tapes religiously.\nJoined local tournaments at age 12.",
    "Comes from a small coastal town.\nLearned basketball from older cousins.\nDreams of going pro to inspire local kids.",
    "Was originally a soccer player.\nSwitched to basketball after a growth spurt.\nKnown for insane stamina on the court.",
    "Raised in a family of musicians.\nUses rhythm to perfect dribbling.\nBrings headphones to every warm-up.",
    "Started out as the shortest kid on the team.\nTrained daily before school.\nNow dominates with speed and grit.",
    "Spent childhood moving city to city.\nBasketball became the constant.\nHas a calm, adaptable playstyle.",
    "Grew up watching streetball legends.\nPracticed fancy moves on city courts.\nLoves flash almost as much as winning.",
    "Parents wanted them to study medicine.\nPracticed secretly at night.\nNow chasing their true passion.",
    "Has a black belt in karate.\nFootwork is their hidden weapon.\nOpponents never see the spin move coming.",
    "Comes from a big sports family.\nBut they’re the only hooper.\nDetermined to prove basketball was the right choice.",
    "Worked part-time in a warehouse.\nUsed lifting boxes as strength training.\nHas surprising power in the paint.",
    "Was bullied for being too quiet.\nCoach saw potential and recruited them.\nDiscovered confidence through basketball.",
    "Lives on a farm outside town.\nShot hoops using a rim on the barn.\nStrong arms from chores give great rebounds.",
    "Former chess tournament player.\nApproaches plays like strategy puzzles.\nAlways two moves ahead.",
    "Lost their shoes during first tryouts.\nPlayed barefoot and impressed everyone.\nNow known for fearless hustle.",
    "Used to draw comics of basketball heroes.\nImagined themselves as one.\nNow they’re creating their own story.",
    "Grew up idolizing a retired legend.\nMet them once at a camp.\nStill carries the autograph for luck.",
    "Worked as a bike courier.\nNavigation gave them crazy court awareness.\nPasses that thread the needle.",
    "Was terrified of public crowds.\nStarted playing to overcome it.\nNow thrives in loud arenas.",
    "Makes their own pre-game energy bars.\nTeam swears they work magic.\nAlways ready to share… for an assist."
};
    }
}
