using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BoxDumper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            T_OutPath.Text = System.Windows.Forms.Application.StartupPath;
            C_Format.SelectedIndex = 0;
            C_Start.SelectedIndex = 0;
            C_End.SelectedIndex = 0;
            C_DStart.SelectedIndex = 0;
            T_Dialog.Text = "Mass Dumper - By Kaphotics and OmegaDonut\r\n\r\nhttp://projectpokemon.org/\r\n\r\nContact @ Forums or (preferably) IRC.";

            C_Format.Items.AddRange(new object[] { "Truck" });
        }
        public byte[] boxfile = new Byte[0x10009C];
        public byte[] savekey = new Byte[0x10009C];
        public byte[] boxkey = new Byte[6960]; // 232*30
        public byte[] blankekx = new Byte[232];
        public string binsave = "Save File|*.sav;*.bin";
        public string modestring = "";

        // Array Manipulation
        private byte[] unshufflearray(byte[] pkx, uint sv)
        {
            byte[] ekx = new Byte[232];
            for (int i = 0; i < 8; i++)
            {
                ekx[i] = pkx[i];
            }

            // Now to shuffle the blocks

            // Define Shuffle Order Structure
            var aloc = new byte[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3 };
            var bloc = new byte[] { 1, 1, 2, 3, 2, 3, 0, 0, 0, 0, 0, 0, 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2 };
            var cloc = new byte[] { 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2, 0, 0, 0, 0, 0, 0, 3, 2, 3, 2, 1, 1 };
            var dloc = new byte[] { 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 0, 0, 0, 0, 0, 0 };

            // Get Shuffle Order
            var shlog = new byte[] { aloc[sv], bloc[sv], cloc[sv], dloc[sv] };

            // UnShuffle Away!
            for (int b = 0; b < 4; b++)
            {
                for (int i = 0; i < 56; i++)
                {
                    ekx[8 + 56 * b + i] = pkx[8 + 56 * shlog[b] + i];
                }
            }

            // Fill the Battle Stats back
            if (pkx.Length > 232)
            {
                for (int i = 232; i < 260; i++)
                {
                    ekx[i] = pkx[i];
                }
            }
            return ekx;
        }
        private byte[] shufflearray(byte[] pkx, uint sv)
        {
            byte[] ekx = new Byte[232];
            for (int i = 0; i < 8; i++)
            {
                ekx[i] = pkx[i];
            }

            // Now to shuffle the blocks

            // Define Shuffle Order Structure
            var aloc = new byte[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3, 1, 1, 2, 3, 2, 3 };
            var bloc = new byte[] { 1, 1, 2, 3, 2, 3, 0, 0, 0, 0, 0, 0, 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2 };
            var cloc = new byte[] { 2, 3, 1, 1, 3, 2, 2, 3, 1, 1, 3, 2, 0, 0, 0, 0, 0, 0, 3, 2, 3, 2, 1, 1 };
            var dloc = new byte[] { 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 3, 2, 3, 2, 1, 1, 0, 0, 0, 0, 0, 0 };

            // Get Shuffle Order
            var shlog = new byte[] { aloc[sv], bloc[sv], cloc[sv], dloc[sv] };

            // Shuffle Away!
            for (int b = 0; b < 4; b++)
            {
                for (int i = 0; i < 56; i++)
                {
                    ekx[8 + 56 * shlog[b] + i] = pkx[8 + 56 * b + i];
                }
            }

            // Fill the Battle Stats back
            if (pkx.Length > 232)
            {
                for (int i = 232; i < 260; i++)
                {
                    ekx[i] = pkx[i];
                }
            }
            return ekx;
        }
        private byte[] decryptarray(byte[] ekx)
        {
            byte[] pkx = ekx;
            uint pv = (uint)ekx[0] + (uint)((ekx[1] << 8)) + (uint)((ekx[2]) << 16) + (uint)((ekx[3]) << 24);
            uint sv = (((pv & 0x3E000) >> 0xD) % 24);

            uint seed = pv;
            // Decrypt Blocks with RNG Seed
            for (int i = 8; i < 232; i += 2)
            {
                int pre = pkx[i] + ((pkx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                pkx[i] = (byte)((post) & 0xFF);
                pkx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }
            // Deshuffle
            pkx = unshufflearray(pkx, sv);

            return pkx;
        }
        private byte[] encryptarray(byte[] pkx)
        {
            // Shuffle
            uint pv = (uint)pkx[0] + (uint)((pkx[1] << 8)) + (uint)((pkx[2]) << 16) + (uint)((pkx[3]) << 24);
            uint sv = (((pv & 0x3E000) >> 0xD) % 24);

            var encrypt_sv = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 12, 18, 13, 19, 8, 10, 14, 20, 16, 22, 9, 11, 15, 21, 17, 23 };

            sv = encrypt_sv[sv];

            byte[] ekx = shufflearray(pkx, sv);

            uint seed = pv;
            // Encrypt Blocks with RNG Seed
            for (int i = 8; i < 232; i += 2)
            {
                int pre = ekx[i] + ((ekx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                ekx[i] = (byte)((post) & 0xFF);
                ekx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }

            // Encrypt the Party Stats
            seed = pv;
            for (int i = 232; i < 260; i += 2)
            {
                int pre = ekx[i] + ((ekx[i + 1]) << 8);
                seed = LCRNG(seed);
                int seedxor = (int)((seed) >> 16);
                int post = (pre ^ seedxor);
                ekx[i] = (byte)((post) & 0xFF);
                ekx[i + 1] = (byte)(((post) >> 8) & 0xFF);
            }

            // Done
            return ekx;
        }
        private string getspecies(int species)
        {
            string[] spectable = new string[] { "None", "Bulbasaur", "Ivysaur", "Venusaur", "Charmander", "Charmeleon", "Charizard", "Squirtle", "Wartortle", "Blastoise", "Caterpie", "Metapod", "Butterfree", "Weedle", "Kakuna", "Beedrill", "Pidgey", "Pidgeotto", "Pidgeot", "Rattata", "Raticate", "Spearow", "Fearow", "Ekans", "Arbok", "Pikachu", "Raichu", "Sandshrew", "Sandslash", "Nidoran♀", "Nidorina", "Nidoqueen", "Nidoran♂", "Nidorino", "Nidoking", "Clefairy", "Clefable", "Vulpix", "Ninetales", "Jigglypuff", "Wigglytuff", "Zubat", "Golbat", "Oddish", "Gloom", "Vileplume", "Paras", "Parasect", "Venonat", "Venomoth", "Diglett", "Dugtrio", "Meowth", "Persian", "Psyduck", "Golduck", "Mankey", "Primeape", "Growlithe", "Arcanine", "Poliwag", "Poliwhirl", "Poliwrath", "Abra", "Kadabra", "Alakazam", "Machop", "Machoke", "Machamp", "Bellsprout", "Weepinbell", "Victreebel", "Tentacool", "Tentacruel", "Geodude", "Graveler", "Golem", "Ponyta", "Rapidash", "Slowpoke", "Slowbro", "Magnemite", "Magneton", "Farfetchd", "Doduo", "Dodrio", "Seel", "Dewgong", "Grimer", "Muk", "Shellder", "Cloyster", "Gastly", "Haunter", "Gengar", "Onix", "Drowzee", "Hypno", "Krabby", "Kingler", "Voltorb", "Electrode", "Exeggcute", "Exeggutor", "Cubone", "Marowak", "Hitmonlee", "Hitmonchan", "Lickitung", "Koffing", "Weezing", "Rhyhorn", "Rhydon", "Chansey", "Tangela", "Kangaskhan", "Horsea", "Seadra", "Goldeen", "Seaking", "Staryu", "Starmie", "Mr. Mime", "Scyther", "Jynx", "Electabuzz", "Magmar", "Pinsir", "Tauros", "Magikarp", "Gyarados", "Lapras", "Ditto", "Eevee", "Vaporeon", "Jolteon", "Flareon", "Porygon", "Omanyte", "Omastar", "Kabuto", "Kabutops", "Aerodactyl", "Snorlax", "Articuno", "Zapdos", "Moltres", "Dratini", "Dragonair", "Dragonite", "Mewtwo", "Mew", "Chikorita", "Bayleef", "Meganium", "Cyndaquil", "Quilava", "Typhlosion", "Totodile", "Croconaw", "Feraligatr", "Sentret", "Furret", "Hoothoot", "Noctowl", "Ledyba", "Ledian", "Spinarak", "Ariados", "Crobat", "Chinchou", "Lanturn", "Pichu", "Cleffa", "Igglybuff", "Togepi", "Togetic", "Natu", "Xatu", "Mareep", "Flaaffy", "Ampharos", "Bellossom", "Marill", "Azumarill", "Sudowoodo", "Politoed", "Hoppip", "Skiploom", "Jumpluff", "Aipom", "Sunkern", "Sunflora", "Yanma", "Wooper", "Quagsire", "Espeon", "Umbreon", "Murkrow", "Slowking", "Misdreavus", "Unown", "Wobbuffet", "Girafarig", "Pineco", "Forretress", "Dunsparce", "Gligar", "Steelix", "Snubbull", "Granbull", "Qwilfish", "Scizor", "Shuckle", "Heracross", "Sneasel", "Teddiursa", "Ursaring", "Slugma", "Magcargo", "Swinub", "Piloswine", "Corsola", "Remoraid", "Octillery", "Delibird", "Mantine", "Skarmory", "Houndour", "Houndoom", "Kingdra", "Phanpy", "Donphan", "Porygon2", "Stantler", "Smeargle", "Tyrogue", "Hitmontop", "Smoochum", "Elekid", "Magby", "Miltank", "Blissey", "Raikou", "Entei", "Suicune", "Larvitar", "Pupitar", "Tyranitar", "Lugia", "Ho-Oh", "Celebi", "Treecko", "Grovyle", "Sceptile", "Torchic", "Combusken", "Blaziken", "Mudkip", "Marshtomp", "Swampert", "Poochyena", "Mightyena", "Zigzagoon", "Linoone", "Wurmple", "Silcoon", "Beautifly", "Cascoon", "Dustox", "Lotad", "Lombre", "Ludicolo", "Seedot", "Nuzleaf", "Shiftry", "Taillow", "Swellow", "Wingull", "Pelipper", "Ralts", "Kirlia", "Gardevoir", "Surskit", "Masquerain", "Shroomish", "Breloom", "Slakoth", "Vigoroth", "Slaking", "Nincada", "Ninjask", "Shedinja", "Whismur", "Loudred", "Exploud", "Makuhita", "Hariyama", "Azurill", "Nosepass", "Skitty", "Delcatty", "Sableye", "Mawile", "Aron", "Lairon", "Aggron", "Meditite", "Medicham", "Electrike", "Manectric", "Plusle", "Minun", "Volbeat", "Illumise", "Roselia", "Gulpin", "Swalot", "Carvanha", "Sharpedo", "Wailmer", "Wailord", "Numel", "Camerupt", "Torkoal", "Spoink", "Grumpig", "Spinda", "Trapinch", "Vibrava", "Flygon", "Cacnea", "Cacturne", "Swablu", "Altaria", "Zangoose", "Seviper", "Lunatone", "Solrock", "Barboach", "Whiscash", "Corphish", "Crawdaunt", "Baltoy", "Claydol", "Lileep", "Cradily", "Anorith", "Armaldo", "Feebas", "Milotic", "Castform", "Kecleon", "Shuppet", "Banette", "Duskull", "Dusclops", "Tropius", "Chimecho", "Absol", "Wynaut", "Snorunt", "Glalie", "Spheal", "Sealeo", "Walrein", "Clamperl", "Huntail", "Gorebyss", "Relicanth", "Luvdisc", "Bagon", "Shelgon", "Salamence", "Beldum", "Metang", "Metagross", "Regirock", "Regice", "Registeel", "Latias", "Latios", "Kyogre", "Groudon", "Rayquaza", "Jirachi", "Deoxys", "Turtwig", "Grotle", "Torterra", "Chimchar", "Monferno", "Infernape", "Piplup", "Prinplup", "Empoleon", "Starly", "Staravia", "Staraptor", "Bidoof", "Bibarel", "Kricketot", "Kricketune", "Shinx", "Luxio", "Luxray", "Budew", "Roserade", "Cranidos", "Rampardos", "Shieldon", "Bastiodon", "Burmy", "Wormadam", "Mothim", "Combee", "Vespiquen", "Pachirisu", "Buizel", "Floatzel", "Cherubi", "Cherrim", "Shellos", "Gastrodon", "Ambipom", "Drifloon", "Drifblim", "Buneary", "Lopunny", "Mismagius", "Honchkrow", "Glameow", "Purugly", "Chingling", "Stunky", "Skuntank", "Bronzor", "Bronzong", "Bonsly", "Mime Jr.", "Happiny", "Chatot", "Spiritomb", "Gible", "Gabite", "Garchomp", "Munchlax", "Riolu", "Lucario", "Hippopotas", "Hippowdon", "Skorupi", "Drapion", "Croagunk", "Toxicroak", "Carnivine", "Finneon", "Lumineon", "Mantyke", "Snover", "Abomasnow", "Weavile", "Magnezone", "Lickilicky", "Rhyperior", "Tangrowth", "Electivire", "Magmortar", "Togekiss", "Yanmega", "Leafeon", "Glaceon", "Gliscor", "Mamoswine", "Porygon-Z", "Gallade", "Probopass", "Dusknoir", "Froslass", "Rotom", "Uxie", "Mesprit", "Azelf", "Dialga", "Palkia", "Heatran", "Regigigas", "Giratina", "Cresselia", "Phione", "Manaphy", "Darkrai", "Shaymin", "Arceus", "Victini", "Snivy", "Servine", "Serperior", "Tepig", "Pignite", "Emboar", "Oshawott", "Dewott", "Samurott", "Patrat", "Watchog", "Lillipup", "Herdier", "Stoutland", "Purrloin", "Liepard", "Pansage", "Simisage", "Pansear", "Simisear", "Panpour", "Simipour", "Munna", "Musharna", "Pidove", "Tranquill", "Unfezant", "Blitzle", "Zebstrika", "Roggenrola", "Boldore", "Gigalith", "Woobat", "Swoobat", "Drilbur", "Excadrill", "Audino", "Timburr", "Gurdurr", "Conkeldurr", "Tympole", "Palpitoad", "Seismitoad", "Throh", "Sawk", "Sewaddle", "Swadloon", "Leavanny", "Venipede", "Whirlipede", "Scolipede", "Cottonee", "Whimsicott", "Petilil", "Lilligant", "Basculin", "Sandile", "Krokorok", "Krookodile", "Darumaka", "Darmanitan", "Maractus", "Dwebble", "Crustle", "Scraggy", "Scrafty", "Sigilyph", "Yamask", "Cofagrigus", "Tirtouga", "Carracosta", "Archen", "Archeops", "Trubbish", "Garbodor", "Zorua", "Zoroark", "Minccino", "Cinccino", "Gothita", "Gothorita", "Gothitelle", "Solosis", "Duosion", "Reuniclus", "Ducklett", "Swanna", "Vanillite", "Vanillish", "Vanilluxe", "Deerling", "Sawsbuck", "Emolga", "Karrablast", "Escavalier", "Foongus", "Amoonguss", "Frillish", "Jellicent", "Alomomola", "Joltik", "Galvantula", "Ferroseed", "Ferrothorn", "Klink", "Klang", "Klinklang", "Tynamo", "Eelektrik", "Eelektross", "Elgyem", "Beheeyem", "Litwick", "Lampent", "Chandelure", "Axew", "Fraxure", "Haxorus", "Cubchoo", "Beartic", "Cryogonal", "Shelmet", "Accelgor", "Stunfisk", "Mienfoo", "Mienshao", "Druddigon", "Golett", "Golurk", "Pawniard", "Bisharp", "Bouffalant", "Rufflet", "Braviary", "Vullaby", "Mandibuzz", "Heatmor", "Durant", "Deino", "Zweilous", "Hydreigon", "Larvesta", "Volcarona", "Cobalion", "Terrakion", "Virizion", "Tornadus", "Thundurus", "Reshiram", "Zekrom", "Landorus", "Kyurem", "Keldeo", "Meloetta", "Genesect", "Chespin", "Quilladin", "Chesnaught", "Fennekin", "Braixen", "Delphox", "Froakie", "Frogadier", "Greninja", "Bunnelby", "Diggersby", "Fletchling", "Fletchinder", "Talonflame", "Scatterbug", "Spewpa", "Vivillon", "Litleo", "Pyroar", "Flabébé", "Floette", "Florges", "Skiddo", "Gogoat", "Pancham", "Pangoro", "Furfrou", "Espurr", "Meowstic", "Honedge", "Doublade", "Aegislash", "Spritzee", "Aromatisse", "Swirlix", "Slurpuff", "Inkay", "Malamar", "Binacle", "Barbaracle", "Skrelp", "Dragalge", "Clauncher", "Clawitzer", "Helioptile", "Heliolisk", "Tyrunt", "Tyrantrum", "Amaura", "Aurorus", "Sylveon", "Hawlucha", "Dedenne", "Carbink", "Goomy", "Sliggoo", "Goodra", "Klefki", "Phantump", "Trevenant", "Pumpkaboo", "Gourgeist", "Bergmite", "Avalugg", "Noibat", "Noivern", "Xerneas", "Yveltal", "Zygarde", "Diancie", "Hoopa", "Volcanion" };
            try
            {
                return spectable[species];
            }
            catch { return "Error"; }
        }
        private string getmove(byte[] buff, int offset)
        {
            string[] movetable = new string[] { "", "Pound", "Karate Chop", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch", "Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust", "Wing Attack", "Whirlwind", "Fly", "Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack", "Headbutt", "Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip", "Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom", "Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard", "Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss", "Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder", "Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake", "Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage", "Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray", "Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move", "Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift", "Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas", "Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave", "Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen", "Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web", "Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal", "Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss", "Belly Drum", "Sludge Bomb", "Mud-Slap", "Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On", "Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark", "Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard", "Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin", "Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight", "Hidden Power", "Cross Chop", "Twister", "Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash", "Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment", "Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt", "Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge", "Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch", "Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick", "Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash", "Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound", "Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold", "Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up", "Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance", "Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost", "Roost", "Gravity", "Miracle Eye", "Wake-Up Slap", "Hammer Arm", "Gyro Ball", "Healing Wish", "Brine", "Natural Gift", "Feint", "Pluck", "Tailwind", "Acupressure", "Metal Burst", "U-turn", "Close Combat", "Payback", "Assurance", "Embargo", "Fling", "Psycho Shift", "Trump Card", "Heal Block", "Wring Out", "Power Trick", "Gastro Acid", "Lucky Chant", "Me First", "Copycat", "Power Swap", "Guard Swap", "Punishment", "Last Resort", "Worry Seed", "Sucker Punch", "Toxic Spikes", "Heart Swap", "Aqua Ring", "Magnet Rise", "Flare Blitz", "Force Palm", "Aura Sphere", "Rock Polish", "Poison Jab", "Dark Pulse", "Night Slash", "Aqua Tail", "Seed Bomb", "Air Slash", "X-Scissor", "Bug Buzz", "Dragon Pulse", "Dragon Rush", "Power Gem", "Drain Punch", "Vacuum Wave", "Focus Blast", "Energy Ball", "Brave Bird", "Earth Power", "Switcheroo", "Giga Impact", "Nasty Plot", "Bullet Punch", "Avalanche", "Ice Shard", "Shadow Claw", "Thunder Fang", "Ice Fang", "Fire Fang", "Shadow Sneak", "Mud Bomb", "Psycho Cut", "Zen Headbutt", "Mirror Shot", "Flash Cannon", "Rock Climb", "Defog", "Trick Room", "Draco Meteor", "Discharge", "Lava Plume", "Leaf Storm", "Power Whip", "Rock Wrecker", "Cross Poison", "Gunk Shot", "Iron Head", "Magnet Bomb", "Stone Edge", "Captivate", "Stealth Rock", "Grass Knot", "Chatter", "Judgment", "Bug Bite", "Charge Beam", "Wood Hammer", "Aqua Jet", "Attack Order", "Defend Order", "Heal Order", "Head Smash", "Double Hit", "Roar of Time", "Spacial Rend", "Lunar Dance", "Crush Grip", "Magma Storm", "Dark Void", "Seed Flare", "Ominous Wind", "Shadow Force", "Hone Claws", "Wide Guard", "Guard Split", "Power Split", "Wonder Room", "Psyshock", "Venoshock", "Autotomize", "Rage Powder", "Telekinesis", "Magic Room", "Smack Down", "Storm Throw", "Flame Burst", "Sludge Wave", "Quiver Dance", "Heavy Slam", "Synchronoise", "Electro Ball", "Soak", "Flame Charge", "Coil", "Low Sweep", "Acid Spray", "Foul Play", "Simple Beam", "Entrainment", "After You", "Round", "Echoed Voice", "Chip Away", "Clear Smog", "Stored Power", "Quick Guard", "Ally Switch", "Scald", "Shell Smash", "Heal Pulse", "Hex", "Sky Drop", "Shift Gear", "Circle Throw", "Incinerate", "Quash", "Acrobatics", "Reflect Type", "Retaliate", "Final Gambit", "Bestow", "Inferno", "Water Pledge", "Fire Pledge", "Grass Pledge", "Volt Switch", "Struggle Bug", "Bulldoze", "Frost Breath", "Dragon Tail", "Work Up", "Electroweb", "Wild Charge", "Drill Run", "Dual Chop", "Heart Stamp", "Horn Leech", "Sacred Sword", "Razor Shell", "Heat Crash", "Leaf Tornado", "Steamroller", "Cotton Guard", "Night Daze", "Psystrike", "Tail Slap", "Hurricane", "Head Charge", "Gear Grind", "Searing Shot", "Techno Blast", "Relic Song", "Secret Sword", "Glaciate", "Bolt Strike", "Blue Flare", "Fiery Dance", "Freeze Shock", "Ice Burn", "Snarl", "Icicle Crash", "V-create", "Fusion Flare", "Fusion Bolt", "Flying Press", "Mat Block", "Belch", "Rototiller", "Sticky Web", "Fell Stinger", "Phantom Force", "Trick-or-Treat", "Noble Roar", "Ion Deluge", "Parabolic Charge", "Forest's Curse", "Petal Blizzard", "Freeze-Dry", "Disarming Voice", "Parting Shot", "Topsy-Turvy", "Draining Kiss", "Crafty Shield", "Flower Shield", "Grassy Terrain", "Misty Terrain", "Electrify", "Play Rough", "Fairy Wind", "Moonblast", "Boomburst", "Fairy Lock", "King's Shield", "Play Nice", "Confide", "'-591-", "'-592-", "'-593-", "Water Shuriken", "Mystical Fire", "Spiky Shield", "Aromatic Mist", "Eerie Impulse", "Venom Drench", "Powder", "Geomancy", "Magnetic Flux", "Happy Hour", "Electric Terrain", "Dazzling Gleam", "Celebrate", "'-607-", "Baby-Doll Eyes", "Nuzzle", "'-610-", "Infestation", "Power-Up Punch", "Oblivion Wing", "'-614-", "'-615-", "Land's Wrath", "'-617", "'-618-", "'-619-", "'-620-", };
            try
            {
                int move = buff[offset] + buff[offset+1]*0x100;
                return movetable[move];
            }
            catch { return "Error";}
        }
        private string getivs(byte[] buff, uint sv)
        {
            int IV32 = buff[0x77] * 0x1000000 + buff[0x76] * 0x10000 + buff[0x75] * 0x100 + buff[0x74];
            int HP_IV = IV32 & 0x1F;
            int ATK_IV = (IV32 >> 5) & 0x1F;
            int DEF_IV = (IV32 >> 10) & 0x1F;
            int SPE_IV = (IV32 >> 15) & 0x1F;
            int SPA_IV = (IV32 >> 20) & 0x1F;
            int SPD_IV = (IV32 >> 25) & 0x1F;

            string ivs = "";
            ivs += HP_IV.ToString("00") + ".";
            ivs += ATK_IV.ToString("00") + ".";
            ivs += DEF_IV.ToString("00") + ".";
            ivs += SPA_IV.ToString("00") + ".";
            ivs += SPD_IV.ToString("00") + ".";
            ivs += SPE_IV.ToString("00");

            int isegg = (IV32 >> 30) & 1;
            if (isegg == 1)
            {
                ivs += " [" + sv.ToString("0000") + "]";
            }
            else if (C_Format.Text == "TSV")
            {
                // Not an Egg. Return TSV instead.
                uint TID = (uint)(buff[0x0C] + buff[0x0D] * 0x100);
                uint SID = (uint)(buff[0x0E] + buff[0x0F] * 0x100);
                uint TSV = (TID ^ SID) >> 4;

                ivs += " (" + TSV.ToString("0000") + ")";
            }
            return ivs;
        }
        private string getivs2(byte[] buff, uint sv)
        {
            int IV32 = buff[0x77] * 0x1000000 + buff[0x76] * 0x10000 + buff[0x75] * 0x100 + buff[0x74];
            int HP_IV = IV32 & 0x1F;
            int ATK_IV = (IV32 >> 5) & 0x1F;
            int DEF_IV = (IV32 >> 10) & 0x1F;
            int SPE_IV = (IV32 >> 15) & 0x1F;
            int SPA_IV = (IV32 >> 20) & 0x1F;
            int SPD_IV = (IV32 >> 25) & 0x1F;

            string ivs = "";
            ivs += HP_IV.ToString("00") + ".";
            ivs += ATK_IV.ToString("00") + ".";
            ivs += DEF_IV.ToString("00") + ".";
            ivs += SPA_IV.ToString("00") + ".";
            ivs += SPD_IV.ToString("00") + ".";
            ivs += SPE_IV.ToString("00");

            int isegg = (IV32 >> 30) & 1;
            if (isegg == 1)
            {
                ivs += " | " + sv.ToString("0000");
            }
            //else
            //{
            //    // Not an Egg. Return TSV instead.
            //    uint TID = (uint)(buff[0x0C] + buff[0x0D] * 0x100);
            //    uint SID = (uint)(buff[0x0E] + buff[0x0F] * 0x100);
            //    uint TSV = (TID ^ SID) >> 4;

            //    ivs += " (" + TSV.ToString("0000") + ")";
            //}
            return ivs;
        }
        private string getivs3(byte[] buff)
        {
            int IV32 = buff[0x77] * 0x1000000 + buff[0x76] * 0x10000 + buff[0x75] * 0x100 + buff[0x74];
            int HP_IV = IV32 & 0x1F;
            int ATK_IV = (IV32 >> 5) & 0x1F;
            int DEF_IV = (IV32 >> 10) & 0x1F;
            int SPE_IV = (IV32 >> 15) & 0x1F;
            int SPA_IV = (IV32 >> 20) & 0x1F;
            int SPD_IV = (IV32 >> 25) & 0x1F;

            string ivs = "";
            ivs += HP_IV.ToString("00") + ",";
            ivs += ATK_IV.ToString("00") + ",";
            ivs += DEF_IV.ToString("00") + ",";
            ivs += SPA_IV.ToString("00") + ",";
            ivs += SPD_IV.ToString("00") + ",";
            ivs += SPE_IV.ToString("00");

            return ivs;
        }
        private string getevs(byte[] buff)
        {
            int HP_EV = buff[0x1E];
            int ATK_EV = buff[0x1F];
            int DEF_EV = buff[0x20];
            int SPE_EV = buff[0x21];
            int SPA_EV = buff[0x22];
            int SPD_EV = buff[0x23];

            string evs = "";
            evs += HP_EV.ToString() + ",";
            evs += ATK_EV.ToString() + ",";
            evs += DEF_EV.ToString() + ",";
            evs += SPA_EV.ToString() + ",";
            evs += SPD_EV.ToString() + ",";
            evs += SPE_EV.ToString();

            return evs;
        }
        private string getnature(byte[] buff)
        {
            int nature = buff[0x1C];
            string[] nattable = new string[] { "Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky" };
            return nattable[nature];
        }
        private string getgender(byte[] buff)
        {
            string g = "";
            int genderflag = (buff[0x1D] >> 1) & 0x3;
            if (genderflag == 0)
            {
                // Gender = Male
                g = " (M)";
            }
            else if (genderflag == 1)
            {
                // Gender = Female
                g = " (F)";
            }
            else { g = ""; }
            return g;
        }
        private string getability(byte[] buff)
        {
            int ability = buff[0x14];
            string[] abiltable = new string[] { "None", "Stench", "Drizzle", "Speed Boost", "Battle Armor", "Sturdy", "Damp", "Limber", "Sand Veil", "Static", "Volt Absorb", "Water Absorb", "Oblivious", "Cloud Nine", "Compound Eyes", "Insomnia", "Color Change", "Immunity", "Flash Fire", "Shield Dust", "Own Tempo", "Suction Cups", "Intimidate", "Shadow Tag", "Rough Skin", "Wonder Guard", "Levitate", "Effect Spore", "Synchronize", "Clear Body", "Natural Cure", "Lightning Rod", "Serene Grace", "Swift Swim", "Chlorophyll", "Illuminate", "Trace", "Huge Power", "Poison Point", "Inner Focus", "Magma Armor", "Water Veil", "Magnet Pull", "Soundproof", "Rain Dish", "Sand Stream", "Pressure", "Thick Fat", "Early Bird", "Flame Body", "Run Away", "Keen Eye", "Hyper Cutter", "Pickup", "Truant", "Hustle", "Cute Charm", "Plus", "Minus", "Forecast", "Sticky Hold", "Shed Skin", "Guts", "Marvel Scale", "Liquid Ooze", "Overgrow", "Blaze", "Torrent", "Swarm", "Rock Head", "Drought", "Arena Trap", "Vital Spirit", "White Smoke", "Pure Power", "Shell Armor", "Air Lock", "Tangled Feet", "Motor Drive", "Rivalry", "Steadfast", "Snow Cloak", "Gluttony", "Anger Point", "Unburden", "Heatproof", "Simple", "Dry Skin", "Download", "Iron Fist", "Poison Heal", "Adaptability", "Skill Link", "Hydration", "Solar Power", "Quick Feet", "Normalize", "Sniper", "Magic Guard", "No Guard", "Stall", "Technician", "Leaf Guard", "Klutz", "Mold Breaker", "Super Luck", "Aftermath", "Anticipation", "Forewarn", "Unaware", "Tinted Lens", "Filter", "Slow Start", "Scrappy", "Storm Drain", "Ice Body", "Solid Rock", "Snow Warning", "Honey Gather", "Frisk", "Reckless", "Multitype", "Flower Gift", "Bad Dreams", "Pickpocket", "Sheer Force", "Contrary", "Unnerve", "Defiant", "Defeatist", "Cursed Body", "Healer", "Friend Guard", "Weak Armor", "Heavy Metal", "Light Metal", "Multiscale", "Toxic Boost", "Flare Boost", "Harvest", "Telepathy", "Moody", "Overcoat", "Poison Touch", "Regenerator", "Big Pecks", "Sand Rush", "Wonder Skin", "Analytic", "Illusion", "Imposter", "Infiltrator", "Mummy", "Moxie", "Justified", "Rattled", "Magic Bounce", "Sap Sipper", "Prankster", "Sand Force", "Iron Barbs", "Zen Mode", "Victory Star", "Turboblaze", "Teravolt", "Aroma Veil", "Flower Veil", "Cheek Pouch", "Protean", "Fur Coat", "Magician", "Bulletproof", "Competitive", "Strong Jaw", "Refrigerate", "Sweet Veil", "Stance Change", "Gale Wings", "Mega Launcher", "Grass Pelt", "Symbiosis", "Tough Claws", "Pixilate", "Gooey", "-184-", "-185-", "Dark Aura", "Fairy Aura", "Aura Break", "-189-", };
            return abiltable[ability];
        }
        private string bytes2text(byte[] buff, int o)
        {
            string charstring;
            charstring = ((char)(buff[o] + 0x100*buff[o + 1])).ToString();
            for (int i = 1; i <= 12; i++)
            {
                int val = buff[o + 2 * i] + 0x100 * buff[o + 2 * i + 1];
                if (val != 0)
                {
                    charstring += ((char)(val)).ToString();
                }
            }
            return charstring;
        }
        private string getTSV(byte[] buff)
        {
            uint TID = (uint)(buff[0x0C] + buff[0x0D] * 0x100);
            uint SID = (uint)(buff[0x0E] + buff[0x0F] * 0x100);
            uint TSV = (TID ^ SID) >> 4;
            return TSV.ToString("0000");
        }
        private string getball(byte[] buff)
        {
            string[] balltable = new string[] { "Poke Ball (Default)", "Master Ball","Ultra Ball","Great Ball","Poke Ball","Safari Ball","Net Ball","Dive Ball","Nest Ball","Repeat Ball","Timer Ball","Luxury Ball","Premier Ball","Dusk Ball","Heal Ball","Quick Ball","Cherish Ball","Fast Ball","Level Ball","Lure Ball","Heavy Ball","Love Ball","Friend Ball","Moon Ball","Comp Ball","Dream Ball" };

            int ball=buff[0xDC];
            
            try
            {
                return balltable[ball];
            }
            catch { return "Error";}
        }


        private static uint CEXOR(uint seed)
        {
            uint a = 0xDEADBABE;
            uint c = 0x2B9A7B1E;

            seed = (seed * a + c) & 0xFFFFFFFF;
            return seed;
        }
        // Custom Encryption
        private byte[] da(byte[] array)
        {

            {
                // Returns the Encrypted/Decrypted Array of Data
                int al = array.Length;
                // Set Encryption Seed
                uint eseed = (uint)(array[al - 4] + array[al - 3] * 0x100 + array[al - 2] * 0x10000 + array[al - 1] * 0x10000000);
                byte[] nca = new Byte[al];

                // Get our XORCryptor
                uint xc = CEXOR(eseed);
                uint xc0 = (xc & 0xFF);
                uint xc1 = ((xc >> 8) & 0xFF);
                uint xc2 = ((xc >> 16) & 0xFF);
                uint xc3 = ((xc >> 24) & 0xFF);

                // Fill Our New Array
                for (int i = 0; i < (al - 4); i += 4)
                {
                    nca[i + 0] = (byte)(xc0 ^ array[i + 0]);
                    nca[i + 1] = (byte)(xc1 ^ array[i + 1]);
                    nca[i + 2] = (byte)(xc2 ^ array[i + 2]);
                    nca[i + 3] = (byte)(xc3 ^ array[i + 3]);
                }
                // Return the Seed
                nca[al - 4] = array[al - 4];
                nca[al - 3] = array[al - 3];
                nca[al - 2] = array[al - 2];
                nca[al - 1] = array[al - 1];

                return nca;
            }
        }

        // Data Manipulation
        public static uint ToUInt32(String value, int b)
        {
            if (String.IsNullOrEmpty(value))
                return 0;
            return Convert.ToUInt32(value, b);
        }
        private static uint LCRNG(uint seed)
        {
            uint a = 0x41C64E6D;
            uint c = 0x00006073;

            seed = (seed * a + c) & 0xFFFFFFFF;
            return seed;
        }
        private uint getchecksum(byte[] pkx)
        {
            uint chk = 0;
            for (int i = 8; i < 232; i += 2) // Loop through the entire PKX
            {
                chk += (uint)(pkx[i] + pkx[i + 1] * 0x100);
            }
            return chk & 0xFFFF;
        }

        private void refreshoffset()
        {
            uint os = 0;
            int box = C_DStart.SelectedIndex;
            uint zor = 0;
            if (CHK_ALT.Checked == false)
            {
                zor = 1;
            }
            else { zor = 0; }
            if (boxfile.Length == 0x100000)
            {
                // Headerless
                os += (uint)(0xA6A00 + (232 * 30 * box) - zor * 0x7F000);
            }
            else
            {
                os += (uint)(0xA6A9C + (232 * 30 * box) - zor * 0x7F000);
            }
            T_O.Text = os.ToString("X");
            T_C.Text = (boxkey.Length / (232*30)).ToString();
        }
        private void refresh2()
        {
            uint os1 = 0;
            uint os2 = 0;
            uint zor = 0;
            uint box1 = ToUInt32(C_Start.Text,10);
            uint box2 = ToUInt32(C_End.Text,10);
            if (CHK_ALT2.Checked == false)
            {
                zor = 1;
            }
            if (boxfile.Length == 0x100000)
            {
                // Headerless
                os1 += (uint)(0xA6A00 + (box1 - 1) * (30 * 232) - zor * 0x7F000);
                os2 += (uint)(0xA6A00 + (box2 - 1) * (30 * 232) - zor * 0x7F000);
            }
            else
            {
                os1 += (uint)(0xA6A9C + (box1 - 1) * (30 * 232) - zor * 0x7F000);
                os2 += (uint)(0xA6A9C + (box2 - 1) * (30 * 232) - zor * 0x7F000);
            }
            T_O1.Text = os1.ToString("X");
            T_O2.Text = os2.ToString("X");
        }
        private void fillboxlist()
        {
            C_DStart.Items.Clear();
            // Get Length of Keystream
            int boxes = boxkey.Length / (232 * 30);
            int nobox = 32-boxes;
            for (int i = 1; i <= nobox; i++)
            {
                C_DStart.Items.AddRange(new object[] { i.ToString() });
            }
            C_DStart.SelectedIndex = 0;
        }
        private void logictodump()
        {
            if ((T_SAV.Text != "") && (T_KEY.Text != "") && (T_EKX.Text != "") && (T_O.Text != ""))
            {
                B_DUMP.Enabled = true;
                C_Format.Enabled = true;
            }
            else
            {
                B_DUMP.Enabled = false;
                C_Format.Enabled = false;
            }

            if (T_KEY.Text != "")
            {
                refreshoffset();
                CHK_ALT.Enabled = true;
                CHK_PK6.Enabled = true;
                T_O.Enabled = true;
                C_DStart.Enabled = true;
            }
            else
            {
                CHK_ALT.Enabled = false;
                CHK_PK6.Enabled = false;
                T_O.Enabled = false;
                C_DStart.Enabled = false;
            }
        }
        
        private void B_DUMP_Click(object sender, EventArgs e)
        {
            string result = "";
            int valid = 0;
            int errors = 0;
            string errstr = "";
            //string corruptedindex = "";
            if (T_O.Text == "")
            {
                // Need an offset.
                //MessageBox.Show("No offset entered.", "Error");
                T_Dialog.Text = "Error: No offset entered. Stopping.";
            }
            else
            {
                // Dump Data
                //try
                {
                    string dumppath = T_OutPath.Text;
                    uint offset = ToUInt32(T_O.Text, 16);
                    if (boxkey.Length < (232 * 30))
                    {
                        //MessageBox.Show("Incorrect Box Keystream Length.", "Error");
                        T_Dialog.Text = "Error: Incorrect Box Keystream Length. Stopping.";
                    }
                    else
                    {
                        // Loop through all 30 to dump
                        byte[] boxekx = new Byte[232];
                        byte[] oldboxkey = new Byte[boxkey.Length];
                        for (int i = 0; i < (boxkey.Length); i++)
                        {
                            oldboxkey[i] = boxkey[i];
                        }
                        byte[] blankpkx = new Byte[232];
                        for (int i = 0; i < (232); i++)
                        {
                            blankpkx[i] = blankekx[i];
                        }
                        blankpkx = decryptarray(blankpkx);
                        for (int i = 0; i < (boxkey.Length/232); i++)
                        {
                            for (int j = 0; j < 232; j++)
                            {
                                boxekx[j] = (byte)(boxfile[offset + i * 232 + j] ^ oldboxkey[i * 232 + j]);
                            }

                            // Okay, we have the data. Let's get some data out for a proper filename.
                            // Decrypt the data
                            byte[] esave = new Byte[232];
                            for (int j = 0; j < 232; j++)
                            {
                                esave[j] = boxekx[j];
                            }

                            byte[] pkxdata = decryptarray(boxekx);
                            uint checksum = getchecksum(pkxdata);
                            uint actualsum = (uint)(pkxdata[0x06] + pkxdata[0x07] * 0x100);
                            string location = "B" + (((i / 30) + 1)+C_DStart.SelectedIndex) + " - " + ((i%30)/6 + 1) + "," + (i % 6 + 1);
                            if (checksum != actualsum)
                            {
                                //MessageBox.Show("Keystream Corruption detected for Index " + i + ". Fixing keystream.", "Error");
                                //corruptedindex += location + " - Keystream Corruption Detected\r\n";
                                //File.WriteAllBytes(dumppath + "\\error"+i+".bin", esave);
                                for (int c = i * 232; c < (i + 1) * 232; c++)
                                {
                                    boxkey[c] = (byte)(oldboxkey[c] ^ blankpkx[c % 232]);
                                }

                                byte[] fixedekx = new Byte[232];
                                // Get actual data now
                                for (int j = 0; j < 232; j++)
                                {
                                    fixedekx[j] = (byte)(boxkey[i * 232 + j] ^ boxfile[offset + i * 232 + j]);
                                }
                                for (int z = 0; z < 232; z++)
                                {
                                    pkxdata[z] = fixedekx[z];
                                    esave[z] = fixedekx[z];
                                }
                                pkxdata = decryptarray(pkxdata);
                                checksum = getchecksum(pkxdata);
                                actualsum = (uint)(pkxdata[0x06] + pkxdata[0x07] * 0x100);
                                if (checksum != actualsum)
                                {
                                    //MessageBox.Show("Keystream correction failed for " + i + ". :(");
                                    errors++;

                                    errstr += location + " - Failed: CHK" + "\r\n";
                                    // Undo our changes
                                    for (int z = 0; z < (boxkey.Length); z++)
                                    {
                                        boxkey[z] = (byte)(oldboxkey[z]);
                                    }
                                    continue;
                                }
                                else
                                {   // Save our changes
                                    //MessageBox.Show("Keystream correction passed.");
                                    //corruptedindex += (i + 1) + " - Keystream Corruption Fixed!\r\n";
                                    if (!File.Exists(T_KEY.Text + ".bak"))
                                    {
                                        File.WriteAllBytes(T_KEY.Text + ".bak", da(oldboxkey));
                                    }
                                    File.WriteAllBytes(T_KEY.Text, da(boxkey));
                                }
                            }

                            // Get PID, ShinyValue and Species Name
                            uint PID = (uint)(pkxdata[0x18] + pkxdata[0x19] * 0x100 + pkxdata[0x1A] * 0x10000 + pkxdata[0x1B] * 0x1000000);
                            uint ShinyValue = (((PID & 0xFFFF) ^ (PID >> 16)) >> 4);
                            int species = pkxdata[0x08] + pkxdata[0x09] * 0x100;
                            if (species > 0)
                            {
                                string specname = getspecies(species);
                                if (specname == "Error")
                                {
                                    //MessageBox.Show("Error on index " + i, "Error");
                                    errors++;
                                    errstr += location + " - Species Index: " + species + "\r\n";
                                }

                                {
                                    if (C_Format.SelectedIndex == 0)
                                    {
                                        // Default
                                        modestring = "\r\nBox - Slot - Name - Nature - Ability - Spread - SV";
                                        string filename =
                                        location
                                        + " - "
                                        + specname
                                        + getgender(pkxdata)
                                        + " - "
                                        + getnature(pkxdata)
                                        + " - "
                                        + getability(pkxdata)
                                        + " - "
                                        + getivs(pkxdata, ShinyValue);
                                        result += "\r\n" + filename;
                                    }
                                    else if (C_Format.Text == "Reddit")
                                    {
                                        // Reddit
                                        modestring = "\r\n| Box | Slot | Name | Nature | Ability | Spread | SV\r\n|:--|:--|:--|:--|:--|:--|:--";
                                        location = "B" + (((i / 30) + 1) + C_DStart.SelectedIndex) + " | " + ((i % 30) / 6 + 1) + "," + (i % 6 + 1);
                                        string resultline =
                                            "| " + location +
                                            " | " + specname + getgender(pkxdata) +
                                            " | " + getnature(pkxdata) +
                                            " | " + getability(pkxdata) +
                                            " | " + getivs2(pkxdata, ShinyValue) +
                                            " |"
                                            ;
                                        result += "\r\n" + resultline;
                                    }
                                    else if (C_Format.Text == "TSV")
                                    {
                                        // TSV Checking Mode
                                        location = "B" + (((i / 30) + 1) + C_DStart.SelectedIndex) + " | " + ((i % 30) / 6 + 1) + "," + (i % 6 + 1);
                                        modestring = "\r\n| Box |Slot | Species | OT | TID | TSV\r\n|:--|:--|:--|:--|:--|:--";
                                        string resultline =
                                            "| " + location + // Slot
                                            " | " + specname + getgender(pkxdata) + // Species
                                            " | " + bytes2text(pkxdata, 0xB0) + // OT
                                            " | " + ((uint)(pkxdata[0x0C] + pkxdata[0x0D] * 0x100)).ToString("00000") + // TID
                                            " | " + getTSV(pkxdata) +
                                            " |"
                                            ;
                                        result += "\r\n" + resultline;
                                    }
                                    else if (C_Format.Text == ".csv")
                                    {
                                        modestring = "";
                                        location = (((i / 30) + 1) + C_DStart.SelectedIndex) + "," + ((i % 30) / 6 + 1) + "," + (i % 6 + 1);
                                        string resultline =
                                            location + // Slot
                                            "," + specname + getgender(pkxdata) + // Species Gender
                                            "," + getnature(pkxdata) + // Nature
                                            "," + getability(pkxdata) + // Ability
                                            "," + getball(pkxdata) + // Ball
                                            "," + getivs3(pkxdata) + // IVs
                                            "," + getevs(pkxdata) + // EVs
                                            "," + getmove(pkxdata, 0x5A) +
                                            "," + getmove(pkxdata, 0x5C) +
                                            "," + getmove(pkxdata, 0x5E) +
                                            "," + getmove(pkxdata, 0x60) +
                                            "," + getmove(pkxdata, 0x6A) +
                                            "," + getmove(pkxdata, 0x6C) +
                                            "," + getmove(pkxdata, 0x6E) +
                                            "," + getmove(pkxdata, 0x70) +
                                            "," + bytes2text(pkxdata, 0xB0) + // OT
                                            "," + ((uint)(pkxdata[0x0C] + pkxdata[0x0D] * 0x100)).ToString("00000") +
                                            "," + getTSV(pkxdata) +
                                            "," + ShinyValue.ToString("0000");
                                        result += "\r\n" + resultline;
                                    }
                                    else if (C_Format.Text == "Truck")
                                    {
                                        // Private Dumper                                    
                                        string filename =
                                        location
                                        + " - "
                                        + specname
                                        + getgender(pkxdata)
                                        + " - "
                                        + getnature(pkxdata)
                                        + " - "
                                        + getability(pkxdata)
                                        + " - "
                                        + getivs(pkxdata, ShinyValue);
                                        result += "\r\n" + filename;
                                        if (CHK_PK6.Checked)
                                        {
                                            string path = dumppath + "\\" + filename + ".pk6";
                                            File.WriteAllBytes(path, pkxdata);
                                        }
                                        else
                                        {
                                            string path = dumppath + "\\" + filename + ".ek6";
                                            File.WriteAllBytes(path, esave);
                                        }
                                    }
                                    valid++;
                                }
                            }
                        }
                        // Load the old boxkey as the new one, in case we made any new alterations.
                        for (int i = 0; i < (boxkey.Length / 232); i++)
                        {
                            oldboxkey[i] = boxkey[i];
                        }

                        if (result == "")
                        {
                            result = "Nothing was dumped.";
                        }

                        if (valid > 0)
                        {
                            if (errors > 0)
                            {
                                MessageBox.Show("Partial Dump :|", "Alert");
                            }
                            MessageBox.Show("Successful Dump!", "Alert");
                        }

                        try { Clipboard.SetText(modestring + result); }
                        catch { };
                        T_Dialog.Text = "";
                        if (C_Format.Text == "Truck")
                        {
                            if (CHK_PK6.Checked)
                            {
                                T_Dialog.Text += "All .pk6's dumped to:\n" + dumppath + "\r\n\r\n"; 
                            }
                            else { T_Dialog.Text += "All .ek6's dumped to:\n" + dumppath + "\r\n\r\n"; }
                        }
                        T_Dialog.Text += "Dumped info copied to Clipboard!\r\n";
                        T_Dialog.Text += "Total Dumped: " + valid + "\r\n";
                        T_Dialog.Text += "Empty Slots: " + ((boxkey.Length/(232)) - valid - errors) + "\r\n";

                        //if ((corruptedindex != "") && (COMPILEMODE == "Private"))
                        //{
                        //    T_Dialog.Text += corruptedindex;
                        //}

                        if (errstr != "")
                        {
                            T_Dialog.Text += errstr;
                        }

                        if (errors > 0)
                        {
                            T_Dialog.Text += "Errors: " + errors + "\r\n";
                        }
                        if (C_Format.Text == ".csv")
                        {
                            T_Dialog.Text += "Data exported to CSV:";

                            result = "Box,Row,Column,Species,Nature,Ability,Ball,HP,ATK,DEF,SpA,SpD,Spe,E_HP,E_ATK,E_DEF,E_SpA,E_SpD,E_SpE,Move1,Move2,Move3,Move4,EggMove1,EggMove2,EggMove3,EggMove4,OT,TID,TSV,ESV" + result;

                            SaveFileDialog savecsv = new SaveFileDialog();
                            savecsv.Filter = "Spreadsheet|*.csv";
                            savecsv.FileName = "MassDump.csv";
                            if (savecsv.ShowDialog() == DialogResult.OK)
                            {
                                string path = savecsv.FileName;
                                System.IO.File.WriteAllText(path, result);

                                T_Dialog.Text += "Path: " + path + "\r\n\r\nRefer to this file to see the results.";
                            }
                            else
                            {
                                T_Dialog.Text += "Didn't write to CSV.";
                            }
                        }
                        else
                        {
                            T_Dialog.Text += "\r\nData Dumped: ";
                            T_Dialog.Text += modestring;
                            T_Dialog.Text += result;
                        }
                        valid = 0;

                    }
                }
                //catch (Exception ex)
                //{
                //    string message = "Error while dumping:\n\n" + ex + "\n\nDid you enter everything properly? If not, fix it!";
                //    string caption = "Error";
                //    MessageBox.Show(message, caption);
                //}
            }
        }
        private void B_DumpKeyRange_Click(object sender, EventArgs e)
        {
            DialogResult ynr = MessageBox.Show("This will only work if you have empty boxes for the region you selected. \r\n\r\nIt is suggested that you have empty boxes for your entire save file (by moving to Bank), and that you do this just once to get 1-30 boxes.\r\n\r\nContinue?", "Confirmation", MessageBoxButtons.YesNo);
            if (ynr == DialogResult.No)
            { // Abort
                T_Dialog.Text = "Aborted dumping. \r\n\r\nIt is suggested to have empty boxes for your entire save file when you try to dump a large key!";
            }
            else if (ynr == DialogResult.Yes)
            { 
                // Get the starting data
                int startoffset = (int)ToUInt32(T_O1.Text, 16);
                int endoffset = (int)ToUInt32(T_O2.Text, 16);
                int gap = endoffset - startoffset + (232 * 30);
                byte[] newkeystream = new Byte[gap];

                for (int i = 0; i < gap; i++)
                {
                    newkeystream[i] = (byte)(savekey[startoffset + i] ^ blankekx[(i) % 232]);
                }
                // Keystream is prepared. Prompt saving.
                SaveFileDialog savenewkey = new SaveFileDialog();
                savenewkey.Filter = "Keystream|*.bin";
                savenewkey.FileName = "KS - " + C_Start.Text + "," + C_End.Text + ".bin";
                if (savenewkey.ShowDialog() == DialogResult.OK)
                {
                    string path = savenewkey.FileName;
                    File.WriteAllBytes(path, da(newkeystream));
                    string result = "Dumped Keystream of the Following Boxes:";
                    result += "\r\nBox" + C_Start.Text;
                    if (C_Format.Text == "Truck")
                    {
                        result += " @ " + T_O1.Text;
                    }
                        result +="\r\nBox" + C_End.Text ;
                    if (C_Format.Text == "Truck")
                    {
                        result += " @ " + T_O2.Text;
                    }
                    result+= "\r\n\r\n" + "Saved to File: \r\n" + path;
                    T_Dialog.Text = result;
                }
                else { T_Dialog.Text = "Decided not to save the newfound box keystream."; }
            }
        }

        private void B_OpenBoxSave_Click(object sender, EventArgs e)
        {
            // Open Save File
            OpenFileDialog boxsave = new OpenFileDialog();
            boxsave.Filter = binsave;

            if (boxsave.ShowDialog() == DialogResult.OK)
            {
                string path = boxsave.FileName;
                byte[] input = File.ReadAllBytes(path);
                if ((input.Length == 0x100000) || (input.Length == 0x10009C))
                {
                    boxfile = input;
                    T_SAV.Text = path;
                    refreshoffset();
                }
                else 
                {
                    MessageBox.Show("Incorrect Save File size loaded.\r\n\r\nYou attempted to load:\r\n" + path + "\r\nThis file is 0x" + input.Length.ToString("X") + " bytes.\r\n\r\nPlease make sure you are loading your 1MB save file.\r\n\r\n(Size should be 0x100000 or 0x10009C, for Digital & PS3DS", "Error: Save File Size");
                }
            }
        }
        private void B_OS2_Click(object sender, EventArgs e)
        {
            // Open Save File
            OpenFileDialog keysave = new OpenFileDialog();
            keysave.Filter = binsave;

            if (keysave.ShowDialog() == DialogResult.OK)
            {
                string path = keysave.FileName;
                byte[] input = File.ReadAllBytes(path);
                if ((input.Length == 0x100000) || (input.Length == 0x10009C))
                {
                    savekey = input;
                    T_SAV2.Text = path;
                }
                else
                {
                    MessageBox.Show("Incorrect Save File size loaded.\r\n\r\nYou attempted to load:\r\n" + path + "\r\nThis file is 0x" + input.Length.ToString("X") + " bytes.\r\n\r\nPlease make sure you are loading your 1MB save file.\r\n\r\n(Size should be 0x100000 or 0x10009C, for Digital & PS3DS", "Error: Save File Size");
                }
            }
        }
        private void B_OpenBoxKey_Click(object sender, EventArgs e)
        {
            // Open Key File
            OpenFileDialog boxkeyfile = new OpenFileDialog();
            boxkeyfile.Filter = "Keystream|*.bin";

            if (boxkeyfile.ShowDialog() == DialogResult.OK)
            {
                string path = boxkeyfile.FileName;
                byte[] input = File.ReadAllBytes(path);
                if ((input.Length % (232*30) == 0) && (input.Length <= (232*30*31)))
                {
                    boxkey = da(input);
                    T_KEY.Text = path;
                }
                else
                {
                    MessageBox.Show("Incorrect BoxKey File size loaded.\r\n\r\nYou attempted to load:\r\n" + path, "Error: Key File Size");
                }
            }
        }
        private void B_OpenBlank_Click(object sender, EventArgs e)
        {
            // Open Key File
            OpenFileDialog openblankekx = new OpenFileDialog();
            openblankekx.Filter = "EKX|*.ekx|EK6|*.ek6";

            if (openblankekx.ShowDialog() == DialogResult.OK)
            {
                string path = openblankekx.FileName;

                byte[] input = File.ReadAllBytes(path);
                if ((input.Length == 232))
                {
                    blankekx = da(input);
                    T_EKX.Text = path;
                    T_EKX2.Text = path;
                    C_Start.SelectedIndex = 5;
                    C_End.SelectedIndex = 0;
                    C_Start.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Incorrect Blank File size loaded.\r\n\r\nYou attempted to load:\r\n" + path +"\r\n\r\nThis file is " + input.Length + "bytes, not 232 as required.", "Error: Blank File Size");
                }

            }
        }

        private void B_ChangeOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                T_OutPath.Text = fbd.SelectedPath;
            }
        }

        private void toggledump(object sender, EventArgs e)
        {
            logictodump();
        }
        private void togglekeydump(object sender, EventArgs e)
        {
            if ((T_O1.Text != "") && (T_O2.Text != "") && (T_EKX2.Text != ""))
            {
                if (ToUInt32(C_Start.Text, 10) < ToUInt32(C_End.Text, 10))
                {
                    B_DumpKeyRange.Enabled = true;
                    CHK_ALT2.Enabled = true;
                }
                else
                {
                    B_DumpKeyRange.Enabled = false;
                    CHK_ALT2.Enabled = false;
                }
            }
            else 
            { 
                B_DumpKeyRange.Enabled = false;
                CHK_ALT2.Enabled = false;
            }
        }
        private void updaterange(object sender, EventArgs e)
        {
            refresh2();
        }
        private void switchkey(object sender, EventArgs e)
        {
            logictodump();
            fillboxlist();
        }
        private void switchmode(object sender, EventArgs e)
        {
            if (C_Format.Text != "Truck")
            {
                B_OutPath.Visible = false;
                T_OutPath.Visible = false;
                CHK_PK6.Visible = false;
                T_O.Visible = false;
                L_O.Visible = false;

                T_O1.Visible = false;
                L_O1.Visible = false;
                T_O2.Visible = false;
                L_O2.Visible = false;
            }
            else
            {
                B_OutPath.Visible = true;
                T_OutPath.Visible = true;
                CHK_PK6.Visible = true;
                T_O.Visible = true;
                L_O.Visible = true;

                T_O1.Visible = true;
                L_O1.Visible = true;
                T_O2.Visible = true;
                L_O2.Visible = true;
                changecheck(sender, e);
            }
        }
        private void changecheck(object sender, EventArgs e)
        {
            if (CHK_PK6.Checked)
            {
                T_Dialog.Text = "Switching to Decrypted PKX export mode.\r\nAll files will be saved with the .pk6 extension.";
            }
            else
            {
                T_Dialog.Text = "Switching to Ecrypted PKX export mode.\r\nAll files will be saved with the .ek6 extension.";
            }
        }

        private void enableT2GB(object sender, EventArgs e)
        {
            if (T_SAV2.Text != "")
            {
                groupBox2.Enabled = true;
            }
        }
        private void enableT1GB(object sender, EventArgs e)
        {
            if (T_SAV.Text != "")
            {
                GB1.Enabled = true;
            }
        }

        private void B_About_Click(object sender, EventArgs e)
        {

            string message = "Mass Dumper - By Kaphotics and OmegaDonut\n\nhttp://projectpokemon.org/";
            message += "\r\n\r\nContact @ Forums or (preferably) IRC.";
            string caption = "About";
            MessageBox.Show(message, caption);
        }

        private void changestart(object sender, EventArgs e)
        {
            refreshoffset();
        }
    }
}
