// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using TrackSearchJSON;
//
//    var main = Main.FromJson(jsonString);

namespace TrackSearchJSON
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Main
    {
        [JsonProperty("track_id")]
        public long TrackId { get; set; }

        [JsonProperty("track_mbid")]
        public string TrackMbid { get; set; }

        [JsonProperty("track_isrc")]
        public string TrackIsrc { get; set; }

        [JsonProperty("track_spotify_id")]
        public string TrackSpotifyId { get; set; }

        [JsonProperty("track_soundcloud_id")]
        public string TrackSoundcloudId { get; set; }

        [JsonProperty("track_xboxmusic_id")]
        public string TrackXboxmusicId { get; set; }

        [JsonProperty("track_name")]
        public string TrackName { get; set; }

        [JsonProperty("track_name_translation")]
        public object[] TrackNameTranslation { get; set; }

        [JsonProperty("track_rating")]
        public long TrackRating { get; set; }

        [JsonProperty("track_length")]
        public long TrackLength { get; set; }

        [JsonProperty("commontrack_id")]
        public long CommontrackId { get; set; }

        [JsonProperty("instrumental")]
        public bool Instrumental { get; set; }

        [JsonProperty("explicit")]
        public long Explicit { get; set; }

        [JsonProperty("has_lyrics")]
        public bool HasLyrics { get; set; }

        [JsonProperty("has_lyrics_crowd")]
        public long HasLyricsCrowd { get; set; }

        [JsonProperty("has_subtitles")]
        public bool HasSubtitles { get; set; }

        [JsonProperty("has_richsync")]
        public long HasRichsync { get; set; }

        [JsonProperty("num_favourite")]
        public long NumFavourite { get; set; }

        [JsonProperty("lyrics_id")]
        public long LyricsId { get; set; }

        [JsonProperty("subtitle_id")]
        public long SubtitleId { get; set; }

        [JsonProperty("album_id")]
        public long AlbumId { get; set; }

        [JsonProperty("album_name")]
        public string AlbumName { get; set; }

        [JsonProperty("artist_id")]
        public long ArtistId { get; set; }

        [JsonProperty("artist_mbid")]
        public string ArtistMbid { get; set; }

        [JsonProperty("artist_name")]
        public string ArtistName { get; set; }

        [JsonProperty("album_coverart_100x100")]
        public string AlbumCoverart100X100 { get; set; }

        [JsonProperty("album_coverart_350x350")]
        public string AlbumCoverart350X350 { get; set; }

        [JsonProperty("album_coverart_500x500")]
        public string AlbumCoverart500X500 { get; set; }

        [JsonProperty("album_coverart_800x800")]
        public string AlbumCoverart800X800 { get; set; }

        [JsonProperty("track_share_url")]
        public string TrackShareUrl { get; set; }

        [JsonProperty("track_edit_url")]
        public string TrackEditUrl { get; set; }

        [JsonProperty("commontrack_vanity_id")]
        public string CommontrackVanityId { get; set; }

        [JsonProperty("restricted")]
        public bool Restricted { get; set; }

        [JsonProperty("first_release_date")]
        public System.DateTimeOffset FirstReleaseDate { get; set; }

        [JsonProperty("updated_time")]
        public System.DateTimeOffset UpdatedTime { get; set; }

        [JsonProperty("primary_genres")]
        public AryGenres PrimaryGenres { get; set; }

        [JsonProperty("secondary_genres")]
        public AryGenres SecondaryGenres { get; set; }
    }

    public partial class AryGenres
    {
        [JsonProperty("music_genre")]
        public MusicGenre[] MusicGenre { get; set; }
    }

    public partial class MusicGenre
    {
        [JsonProperty("music_genre_id")]
        public long MusicGenreId { get; set; }

        [JsonProperty("music_genre_parent_id")]
        public long MusicGenreParentId { get; set; }

        [JsonProperty("music_genre_name")]
        public string MusicGenreName { get; set; }

        [JsonProperty("music_genre_name_extended")]
        public string MusicGenreNameExtended { get; set; }

        [JsonProperty("music_genre_vanity")]
        public string MusicGenreVanity { get; set; }
    }

    public partial class Main
    {
        public static Main[] FromJson(string json) => JsonConvert.DeserializeObject<Main[]>(json, TrackSearchJSON.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Main[] self) => JsonConvert.SerializeObject(self, TrackSearchJSON.Converter.Settings);
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }
}
