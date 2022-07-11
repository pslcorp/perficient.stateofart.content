using Tweetinvi;
using Tweetinvi.Models.V2;

namespace MinimalApi
{
    public interface ITwitterService
    {
        Task<TweetV2> GetTweetById(long id);
        Task<TwitterClient> BuildClient(string CONSUMER_KEY, string CONSUMER_SECRET, string ACCESS_TOKEN, string ACCESS_TOKEN_SECRET);
        Task<TwitterClient> BuildClient();

    }
}
