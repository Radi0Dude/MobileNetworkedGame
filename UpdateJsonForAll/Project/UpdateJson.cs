using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Apis;

namespace TvSeriesModule;

public class TvSeriesService
{
    private readonly IGameApiClient _gameApiClient;

    public TvSeriesService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient;
    }

    // ---- Public Endpoints ----

    [CloudCodeFunction("GetRandomShow")]
    public async Task<string> GetRandomShow(IExecutionContext context)
    {
        var data = await LoadData(context);

        if (data.tvSeries.Count == 0)
            throw new Exception("No shows left!");

        var random = new Random();
        return data.tvSeries[random.Next(data.tvSeries.Count)].id;
    }

    [CloudCodeFunction("RemoveAndGetNewShow")]
    public async Task<string> RemoveAndGetNewShow(IExecutionContext context, string showId)
    {
        await AcquireLock(context);

        try
        {
            var data = await LoadData(context);

            // Check if it was already removed by someone else
            bool stillExists = data.tvSeries.Any(s => s.id == showId);

            if (stillExists)
            {
                data.tvSeries.RemoveAll(s => s.id == showId);
                await SaveData(context, data);
            }

            if (data.tvSeries.Count == 0)
                throw new Exception("No shows left!");

            var random = new Random();
            return data.tvSeries[random.Next(data.tvSeries.Count)].id;
        }
        finally
        {
            await ReleaseLock(context);
        }
    }

    // ---- Lock Helpers ----

    private async Task AcquireLock(IExecutionContext context)
    {
        int attempts = 0;
        int maxAttempts = 20;

        while (attempts < maxAttempts)
        {
            var response = await _gameApiClient.CloudSaveData.GetCustomDataItemsAsync(
                context, context.ProjectId, new List<string> { "removeLock" });

            var lockValue = response.Data.Results.FirstOrDefault()?.Value?.ToString();

            if (lockValue != "locked")
            {
                await SetLock(context, "locked");
                return;
            }

            attempts++;
            await Task.Delay(100);
        }

        throw new Exception("Could not acquire lock, try again later.");
    }

    private async Task ReleaseLock(IExecutionContext context)
    {
        await SetLock(context, "unlocked");
    }

    private async Task SetLock(IExecutionContext context, string value)
    {
        await _gameApiClient.CloudSaveData.SetCustomDataItemsAsync(
            context, context.ProjectId,
            new SetItemBody
            {
                Data = new List<SetItemBody.DataItem>
                {
                    new SetItemBody.DataItem { Key = "removeLock", Value = value }
                }
            });
    }

    // ---- Data Helpers ----

    private async Task<TvSeriesData> LoadData(IExecutionContext context)
    {
        var response = await _gameApiClient.CloudSaveData.GetCustomDataItemsAsync(
            context, context.ProjectId, new List<string> { "TvSeriesDatabaseJSON" }); // custom item name

        var json = response.Data.Results[0].Value.ToString();
        return JsonSerializer.Deserialize<TvSeriesData>(json);
    }

    private async Task SaveData(IExecutionContext context, TvSeriesData data)
    {
        var json = JsonSerializer.Serialize(data);

        await _gameApiClient.CloudSaveData.SetCustomDataItemsAsync(
            context, context.ProjectId,
            new SetItemBody
            {
                Data = new List<SetItemBody.DataItem>
                {
                    new SetItemBody.DataItem { Key = "TvSeriesDatabaseJSON", Value = json } // custom item name
                }
            });
    }
}

// ---- Models ----

public class TvSeriesData
{
    public List<TvShow> tvSeries { get; set; } // matches your "tvSeries" key in the JSON
}

public class TvShow
{
    public string id { get; set; }
    public string title { get; set; }
}