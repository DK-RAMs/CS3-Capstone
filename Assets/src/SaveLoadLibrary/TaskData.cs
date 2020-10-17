using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class TaskData
    {
        private int taskID, startTime, endTime, startDay, endDay;
        private bool completed, firstsuccess;
        private string buildingID;
        

        public TaskData(CitizenTask t)
        {
            taskID = t.TaskID;
            startTime = t.StartTime;
            endTime = t.EndTime;
            startDay = t.StartDay;
            endDay = t.EndDay;
            completed = t.Completed;
            firstsuccess = t.FirstSuccess;
            buildingID = t.taskLocation.ID;
        }
        

        public int TaskID => taskID;

        public int StartDay => startDay;

        public int StartTime => startTime;

        public int EndTime => endTime;

        public int EndDay => endDay;

        public bool Completed => completed;

        public bool FirstSuccess => firstsuccess;

        public string BuildingID => buildingID;
    }
}