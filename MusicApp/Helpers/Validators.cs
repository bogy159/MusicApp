using MusicApp.Models;
using System.Text.RegularExpressions;

namespace MusicApp.Helpers
{
    public static class Validators
    {
        public static string NameCheck(string filename)
        {

            for (int i = 0; i < filename.Length; i++)
            {
                if (filename[i] > sbyte.MaxValue)
                    return "Not ASCII";
                else if (filename[i] == 32)
                {
                    if (filename[i + 1] == 32)
                        return "Double Space";
                    else if (filename[i + 1] != 38 && filename[i + 1] != 45 && ((filename[i + 1] < 65 || filename[i + 1] > 90) && (filename[i + 1] < 48 || filename[i + 1] > 57)))
                        return "After Space";
                }
                else if (filename[i] == 40)
                {
                    if (filename[i + 1] < 65 || filename[i + 1] > 90)
                        return "After (";
                    //else if (filename[filename.Length - 5] != 41)
                    //    return "No )";
                }
            }
            if (!filename.EndsWith(".mp3"))
                return "Not .mp3";
            else if (filename.Split().Count(r => r == "-") == 0)
                return "No -";
            else if (!filename.Contains('(') && filename.Contains(')'))
                return "No (";
            else if (filename.Contains('(') && !filename.Contains(')'))
                return "No )";
            else if (filename.Contains(".."))
                return "Contains ..";
            else if (filename.Count(x => x == '.') <= 1)
                return "Just one .";
            else if (filename[2] != 46 && filename[3] != 46)
                return "First . missing";
            else if (filename[0] < 48 || filename[0] > 57 || filename[1] < 48 || filename[1] > 57)
                return "No numbers";
            else if (filename[3] == 46 && (filename[2] < 48 || filename[2] > 57))
                return "Wrong numbers";
            else if (filename[2] == 46 && ((filename[3] < 65 || filename[3] > 90) && (filename[3] < 48 || filename[3] > 57)))
                return "Capital letter";
            else if (filename[3] == 46 && ((filename[4] < 65 || filename[4] > 90) && (filename[4] < 48 || filename[4] > 57)))
                return "Capital letter 2";
            else if (filename.Split().Count(r => r == "-") > 1)
            {
                if (filename.Contains(" - Cover"))
                {
                }
                else
                    return "Multiple -";
            }
            return "";
        }

        public static string ArtistCheck(Song song)
        {
            string[] name = Regex.Split(song.FileName, " - ");
            string[] artist = name[0].Split(['.'], 2);
            if (song.AlbumArtist == "" || song.AlbumArtist.Contains("ntitled") || song.AlbumArtist.Contains("various") || song.AlbumArtist.Contains("Various") || song.AlbumArtist.Contains("nknown"))
            {
                if (NameCheck(song.FileName) != "")
                    return "Improper file name";
                else if (artist.Length >= 2 && artist[1] != null)
                    return artist[1];
            }
            else if (!string.Equals(song.AlbumArtist, artist[1]))
                return artist[1];
            else if (!string.Equals(song.AlbumArtist, song.ContributingArtist))
                return "Different Artists";

            return "";
        }

        public static string TitleCheck(Song song)
        {
            if (NameCheck(song.FileName) != "")
                return "Improper file name"; 
            string[] newName = Regex.Split(song.FileName, " - ");
            if (newName.Length >= 2 && newName[1] != null)
            {
                string[] newest = Regex.Split(newName[1], "\\.mp3");
                if (!string.Equals(newest[0], song.SongTitle))
                    return newest[0];
            }
            return "";
        }

        public static bool IsValidYear(string? value) => int.TryParse(value, out var year) && year > 1900 && year <= DateTime.Now.Year;
    
        public static bool IsValidPicture(TagLib.IPicture picture)
        {
            return picture != null && picture.Type != TagLib.PictureType.NotAPicture && picture?.Data?.Data != null;
        }
    }
}