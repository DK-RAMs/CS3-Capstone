using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GPGSAuthentication : MonoBehaviour
{
    public static PlayGamesPlatform platform; // only going to initialise this once so don't have to keep doing it for every scene - Z

    // Start is called before the first frame update
    void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build(); // PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableCloudSaving().Build(); for when want to enable cloud saving - Z
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate(); // basically what you call when you log in - Z
        } // if not null then doesn't do anything, ensures we only have the one platform running at a time so updates to leaderboard etc. don't screw with each other if there were multiple instances of it - Z

        // log in
        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Logged in successfully!");
            }
            else
            {
                Debug.Log("FAILED TO LOGIN :(");
            }
        });
    }
}
