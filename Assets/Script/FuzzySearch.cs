using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class FuzzySearch : MonoBehaviour
{

    TextAsset jsonFile;
    MovieList movieList;
    [SerializeField]
    private TMP_InputField searchInputField;

    [SerializeField]
    List<string> currentList = new List<string>();

    private void Start()
    {
        jsonFile = Resources.Load<TextAsset>("movies");
        movieList = JsonUtility.FromJson<MovieList>(jsonFile.text);

    }

    private async Task<List<Movie>> Search(string query)
    {
        query = query.ToLower();

        var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return await Task.Run(() => movieList.movies.Where(m => words.All(w => m.searchTitle.Contains(w))).OrderBy(m => m.searchTitle.StartsWith(query) ? 0 : 1).ThenBy(m => m.searchTitle.Length).Take(20).ToList());

    }

    public async void OnSearchValueChanged()
    {
        string searchTerm = searchInputField.text;
        if (searchTerm.Length < 2) return;

        var results = await Search(searchTerm);
        DisplayResults(results);
    }

    private void DisplayResults(List<Movie> results)
    {
        currentList.Clear();
        foreach (var movie in results) 
        { 
            currentList.Add($"{movie.title} ({movie.year})");
        }
    }

    

    
}
[System.Serializable]
public class Movie
{
    public string id;
    public string title;
    public int year;
    public string searchTitle;
}

[System.Serializable]
public class MovieList
{
    public List<Movie> movies;
}