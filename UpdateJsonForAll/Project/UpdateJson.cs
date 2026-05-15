using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudSave.Model;

namespace TvSeriesModule;

public class ModuleConfig : ICloudCodeSetup
{
    public void Setup(ICloudCodeConfig config)
    {
        config.Dependencies.AddSingleton(GameApiClient.Create());
    }
}

public class TvSeriesService
{
    private const string TvSeriesKey = "tvSeries";
    private const string CustomDataId = "TvSeriesDatabaseJSON"; 
    private readonly IGameApiClient _gameApiClient;

   

    public TvSeriesService(IGameApiClient gameApiClient)
    {
        _gameApiClient = gameApiClient;
    }

    [CloudCodeFunction("RemoveShow")]
    public async Task RemoveShow(IExecutionContext context, string showId)
    {
        var data = await LoadData(context);
        data.tvSeries.RemoveAll(s => s.id == showId);
        await SaveData(context, data);
    }

    private async Task<TvSeriesData> LoadData(IExecutionContext context)
    {
        var response = await _gameApiClient.CloudSaveData.GetCustomItemsAsync(
            context,
            context.ServiceToken,   
            context.ProjectId,
            CustomDataId,           
            new List<string> { TvSeriesKey });

        if (response == null)
            throw new Exception("Response is null");

        if (response.Data == null)
            throw new Exception("Response.Data is null");

        if (response.Data.Results == null)
            throw new Exception("Response.Data.Results is null");

        var item = response.Data.Results.FirstOrDefault();
        if (item?.Value == null)
            return new TvSeriesData { tvSeries = new List<TvShow>() };

        var data = JsonSerializer.Deserialize<TvSeriesData>(item.Value.ToString());
        return data ?? new TvSeriesData { tvSeries = new List<TvShow>() };
    }

    private async Task SaveData(IExecutionContext context, TvSeriesData data)
    {
        var json = JsonSerializer.Serialize(data);

        await _gameApiClient.CloudSaveData.SetCustomItemBatchAsync(
            context,
            context.ServiceToken,   
            context.ProjectId,
            CustomDataId,           
            new SetItemBatchBody(new List<SetItemBody>
            {
                new SetItemBody(TvSeriesKey, json)
            }));
    }
}

public class TvSeriesData
{
    public List<TvShow> tvSeries { get; set; } = new();
}

public class TvShow
{
    public string id { get; set; } = string.Empty;
    public string title { get; set; } = string.Empty;
}