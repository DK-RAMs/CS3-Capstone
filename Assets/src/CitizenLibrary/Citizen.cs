using System;
using System.Collections.ObjectModel;
using src.SaveLoadLibrary;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace src.CitizenLibrary {
    public class Citizen
    {

        string id, name;
        int age, startWork, endWork;
        private double happiness, riskofDeath;
        private bool rebel, hospitalized, dead, infected, wearingMask;

        public Building workLocation, homeLocation;
        
        public enum HealthRisk { Diabetic, Respiratory, Cardial, Old };


        private static Random random = new Random();

        public static double beginRebelVal, stopRebelVal, baseChance, favoriteModifier;

        private CitizenTask favoriteTask;

        private CitizenTask currentTask;
        private Collection<HealthRisk> healthRisks;

        private static Collection<string> names;

        public static readonly string IDPREFIX = "citizen";
        public static int citizenNum = 0;


        #region Constructors
        
        
        public Citizen(string id, string name, double happiness, int age, bool infected, bool wearingMask, bool rebel, bool hospitalized, bool dead)
        {
            citizenNum++;
            this.id = id;
            this.name = name;
            this.age = age;
            this.infected = infected;
            this.rebel = rebel;
            this.happiness = happiness;
            this.hospitalized = hospitalized;
            this.dead = dead;
            this.wearingMask = wearingMask;
            
        }

        public Citizen(CitizenData c) // I give up man, this team blows. Would have been a better project if I was working solo ngl
        {
            id = c.ID;
            name = c.Name;
            generateLastTask(c);
            loadWorkLocation(c);
            happiness = 50;
            name = c.Name;
            age = random.Next(18, 45);
            infected = c.Infected;
            
            if (c.Dead)
            {
                dead = true;
                Game.town.incrementDead();
            }
            
            
            //loadFavoriteTask(); // Need to figure out how we're gonna pull out the building here
            
        }
        public Citizen()
        {
            id = IDPREFIX + citizenNum;
            citizenNum++;
            int nameSelection = random.Next(1);
            if (nameSelection > 0)
            {
                name = "Jane";
            }
            else
            {
                name = "John";
            }
            age = random.Next(18, 65);
            citizenNum++;
            infected = false;
            rebel = false;
            happiness = 50;
            hospitalized = false;
            dead = false;
            wearingMask = false;
            homeLocation = Game.town.Residential[random.Next(Game.town.Residential.Count-1)];
            generateWorkLocation(random.Next(3));
            favoriteTask = new CitizenTask(random, this, true);
            currentTask = new CitizenTask(random, this, false);
            generateCitizenRisk(random.Next(1), random.Next(1), random.Next(1), Game.GAME_MODIFIER);
        }
        
        #endregion

        #region Initialization Methods

        private void generateLastTask(CitizenData c)
        {
            bool buildingFound = false;
            string taskLoc = c.CurrentTaskLoc;
            for (int i = 0; i < Game.town.recreational.Count; i++)
            {
                if (Game.town.recreational[i].ID.Equals(taskLoc))
                {
                    int num = 0;
                    if (c.TaskCompleted)
                    {
                        num = 1;
                    }
                    currentTask = new CitizenTask(c.CurrentTaskId, Game.town.Time, c.EndTime, Game.town.Day, c.EndDay, num, Game.town.recreational[i]);
                    buildingFound = true;
                }
            }
            if (!buildingFound)
            {
                for (int i = 0; i < Game.town.residential.Count; i++)
                {
                    if (Game.town.residential[i].ID.Equals(taskLoc))
                    {
                        int num = 0;
                        if (c.TaskCompleted)
                        {
                            num = 1;
                        }
                        currentTask = new CitizenTask(c.CurrentTaskId, Game.town.Time, c.EndTime, Game.town.Day, c.EndDay, num, Game.town.residential[i]);
                        buildingFound = true;
                    }
                }
            }

            if (!buildingFound)
            {
                for (int i = 0; i < Game.town.essentials.Count; i++)
                {
                    if (Game.town.essentials[i].ID.Equals(taskLoc))
                    {
                        int num = 0;
                        if (c.TaskCompleted)
                        {
                            num = 1;
                        }
                        currentTask = new CitizenTask(c.CurrentTaskId, Game.town.Time, c.EndTime, Game.town.Day, c.EndDay, num, Game.town.essentials[i]);
                        buildingFound = true;
                    }
                }
            }

            if (!buildingFound)
            {
                for (int i = 0; i < Game.town.emergency.Count; i++)
                {
                    if (Game.town.emergency[i].ID.Equals(taskLoc))
                    {
                        int num = 0;
                        if (c.TaskCompleted)
                        {
                            num = 1;
                        }
                        currentTask = new CitizenTask(c.CurrentTaskId, Game.town.Time, c.EndTime, Game.town.Day, c.EndDay, num, Game.town.emergency[i]);
                        buildingFound = true;
                    }
                }
            }

            if (!buildingFound)
            {
                Debug.Log("This game sucks. Can't even load shit. It's like Detroit all over again");
            }
        }
        private void generateCitizenRisk(int diabetic, int respiratory, int cardial, double modifier)
        {
            healthRisks = new Collection<HealthRisk>();
            riskofDeath = Game.town.BaseCitisenRisk*modifier;
            if (diabetic == 1)
            {
                healthRisks.Add(HealthRisk.Diabetic);
                riskofDeath += 1*modifier;
            }
            if (respiratory == 1)
            {
                healthRisks.Add(HealthRisk.Respiratory);
                riskofDeath += 1*modifier;
            }
            if (cardial == 1)
            {
                healthRisks.Add(HealthRisk.Cardial);
                riskofDeath += 1 * modifier;
            }

            if (age > 45)
            {
                healthRisks.Add(HealthRisk.Old);
                riskofDeath += 1 * modifier;
            }
        }
        
        /**
         * Generates the citizen's work location
         */
        private void generateWorkLocation(int val)
        {
            switch (val)
            {
                case 0: // Citizen works at a recreational facility
                    workLocation = Game.town.Recreational[random.Next(Game.town.Recreational.Count - 1)];
                    break;
                case 1: // Citizen works at an essentials building
                    workLocation = Game.town.Essentials[random.Next(Game.town.Essentials.Count - 1)];
                    break;
                case 2: // Citizen works at a hospital
                    workLocation = Game.town.Emergency[random.Next(Game.town.Emergency.Count - 1)];
                    break;
                case 3: // Citizen works from home
                    workLocation = homeLocation;
                    break;
            }
        }

        private void loadWorkLocation(CitizenData c)
        {
            bool buildingFound = false;
            string taskLoc = c.CurrentTaskLoc;
            for (int i = 0; i < Game.town.recreational.Count; i++)
            {
                if (Game.town.recreational[i].ID.Equals(taskLoc))
                {
                    workLocation = Game.town.recreational[i];
                    buildingFound = true;
                }
            }

            if (!buildingFound)
            {
                for (int i = 0; i < Game.town.residential.Count; i++)
                {
                    if (Game.town.recreational[i].ID.Equals(taskLoc))
                    {
                        workLocation = Game.town.residential[i];
                        buildingFound = true;
                    }
                }

                if (!buildingFound)
                {
                    for (int i = 0; i < Game.town.essentials.Count; i++)
                    {
                        if (Game.town.recreational[i].ID.Equals(taskLoc))
                        {
                            workLocation = Game.town.essentials[i];
                            buildingFound = true;
                        }
                    }
                }

                if (!buildingFound)
                {
                    for (int i = 0; i < Game.town.emergency.Count; i++)
                    {
                        if (Game.town.recreational[i].ID.Equals(taskLoc))
                        {
                            workLocation = Game.town.emergency[i];
                            buildingFound = true;
                        }
                    }
                }

                if (!buildingFound)
                {
                    Debug.Log("This game sucks. Mans can't even find a job. Detroit is rough man");
                }

            }
        }

        public void loadPreviousTask(int taskID, int startTime, int endTime, int startDay, int endDay, int completed, Building building)
        {
            currentTask = new CitizenTask(taskID, startTime, endTime, startDay, endDay, completed, building);
        }
        
            private void loadFavoriteTask(int taskID, Building building)
            {
                favoriteTask = new CitizenTask(taskID, 0, 0, 0, 0, 0, building); // Instantiated with Zeros since that data isn't important for loading the citizen's favorite task
            }

            public void initiateTask()
            {
                switch (currentTask.taskBuildingType)
                {
                    case Building.BuildingType.Emergency:
                        break;
                    case Building.BuildingType.Essential:
                        break;
                    case Building.BuildingType.Recreational:
                        break;
                    case Building.BuildingType.Residential:
                        break;
                }
                currentTask.taskLocation.enterBuilding(this);
            }

            #endregion

            #region Citizen Updating Methods
            public void Update()
            {
                if (!hospitalized) // Checks if citizen is hospitalized (i.e. in hospital)
                {
                    if (Game.town.Time % 6 == 0) // Every day at 6am
                    {
                        if (!rebel && Game.town.policyImplemented[1]) // PolicyImplementation[1] - Citizens must wear face masks at all times
                        {
                            wearingMask = true;
                        }
                        else
                        {
                            wearingMask = false;
                        }
                        if (infected && Game.town.Day > 3)
                        {
                            int hospitalizeRoll = rollDice();
                            if (hospitalizeRoll <= (riskofDeath))
                            {
                                hospitalized = true;
                            }
                            else if (currentTask.TaskID == 5 && currentTask.EndTime >= Game.town.Time && currentTask.EndDay >= Game.town.Day) // Citizen is cured once they managed to get through 15 days of self quarantine
                            {
                                infected = false;
                            }
                        }
                    }

                    if (Game.town.Time % 6 == 1)
                    {
                        
                    }
                    
                    updateTask(); // Task must update here. The hospitalization roll needs to be committed before update (since citizen is hospitalized IN the method)
                    if (Game.town.Timer.ElapsedMilliseconds >= Game.UPDATETICKRATE) // Update tick rate is longer than a second. (Maybe 1 hour in game is 1 second) // This is not required since tasks
                    {
                        // Unnecessary check here
                        happiness +=
                            Game.town.BaseDetalHappiness; // Adds the total change to the citizen's happiness to citizen's current happiness

                        if (happiness > 100)
                        {
                            happiness = 100;
                        }
                        else if (happiness < 0)
                        {
                            happiness = 0;
                        }
                        generateRebelFactor();
                    }
                }
                else
                {
                    if (Game.town.Time % 12 == 0) // Every 12 hours, a death roll happens, if the citizen gets hit by it, they die
                    {
                        int deathRoll = rollDice();
                        if (deathRoll <= -1)
                        {
                            Game.town.incrementDead();
                            Debug.LogError("Citizen Died");
                            dead = true;
                        }
                    }

                    if (Game.town.Time >= currentTask.EndTime && Game.town.Day >= currentTask.EndDay) // Citizen managed to recover from the disease
                    {
                        infected = false;
                        hospitalized = false;
                        riskofDeath++;
                        rebel = false;
                        happiness = 85;
                    }
                }
            }


            private void generateRebelFactor()
            {
                if (Game.town.Time % 6 == 0)
                {
                    if (rebel)
                    {
                        if (happiness > stopRebelVal)
                        {
                            int roll = rollDice();
                            int chance =
                                Convert.ToInt16(baseChance + happiness -
                                                stopRebelVal); // As a citizen's happiness increases, the chance of them staying a rebel decreases (stopRebelVal is specified at the beginning of the game.) This roll occurs 
                            if (roll <= chance)
                            {
                                rebel = false;
                            }
                        }
                    }

                    if (!rebel)
                    {
                        if (happiness < beginRebelVal)
                        {
                            int roll = rollDice();
                            int chance =
                                Convert.ToInt16(baseChance + beginRebelVal -
                                                happiness); // As a citizen's happiness decreases, the chance of them rebelling increases.
                            if (roll <= chance)
                            {
                                rebel = true; // Citizen becomes a rebel after being upset for awhile
                            }
                        }
                    }
                }
            }

            private void updateTask()
            {
                currentTask.Update(random, this); // Updates citizen's task
                if (currentTask.Completed)
                {
                    if (currentTask.Equals(favoriteTask))
                    {
                        happiness += currentTask.calculateTaskHappiness(random, rebel, favoriteModifier);
                    }
                    else
                    {
                        happiness += currentTask.calculateTaskHappiness(random, rebel, 1);
                    }

                    currentTask = new CitizenTask(random, this, false); // Generates a new task that isn't a favorite
                }
            }
        
            private int rollDice()
            {
                return random.Next(1, 100);
            }

            #endregion
        
            #region Event Methods

            public void rollHealthEvent(int chance)
            {
                int infectRoll = rollDice();
                if (infectRoll <= chance)
                {
                    infected = true;
                }
            }

            public void applyHappiness(double happiness)
            {
                this.happiness += happiness;
                if (this.happiness > 100)
                {
                    this.happiness = 100;
                }
            }

            public void reset()
            {
                rebel = false;
                happiness = 55;
            }
        
        #endregion
        
        
        #region Getters & Setters

        public bool WearingMask => wearingMask;
        public bool Hospitalized => hospitalized;

        public double RiskofDeath => riskofDeath;

        public bool Dead => dead;

        public bool Infected => infected;

        public string ID => id;

        public string Name => name;

        public bool Rebel => rebel;

        public double Happiness => happiness;

        public int Age => age;

        public CitizenTask CurrentTask => currentTask;

        public CitizenTask FavoriteTask => favoriteTask;

        #endregion
        
        #region Property Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Citizen))
            {
                return false;
            }
            Citizen c = (Citizen) obj;
            return c.id.Equals(id);
        }

        public override int GetHashCode()
        {
            int hash = 11;
            int hashID = hash * 17 + ID.GetHashCode();
            return hashID;
        }

        #endregion

    }
}