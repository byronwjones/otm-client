using BWJ.Web.OTM.Internal;
using System;
using System.ComponentModel;

namespace BWJ.Web.OTM.Models
{
    public enum Language
    {
        [Description("")]
        UnsupportedLanguage = 0,

        [Description("Afrikaans")]
        Afrikaans = 24,
        [Description("Albanian")]
        Albanian = 25,
        [Description("Amharic")]
        Amharic = 2,
        [Description("Arabic")]
        Arabic = 3,
        [Description("Armenian")]
        Armenian = 26,
        [Description("Armenian (West)")]
        Armenian_West = 152,
        [Description("Assyrian")]
        Assyrian = 27,
        [Description("Azerbaijani")]
        Azerbaijani = 29,
        [Description("Balinese")]
        Balinese = 30,
        [Description("Bengali")]
        Bengali = 33,
        [Description("Bislama")]
        Bislama = 34,
        [Description("Bosnian")]
        Bosnian = 123,
        [Description("Bulgarian")]
        Bulgarian = 35,
        [Description("Bunong")]
        Bunong = 143,
        [Description("Cambodian")]
        Cambodian = 4,
        [Description("Cantonese")]
        Cantonese = 5,
        [Description("Catalan")]
        Catalan = 36,
        [Description("Cebuano")]
        Cebuano = 132,
        [Description("Chatino")]
        Chatino = 141,
        [Description("Cherokee")]
        Cherokee = 138,
        [Description("Chichewa")]
        Chichewa = 39,
        [Description("Chinese")]
        Chinese = 6,
        [Description("Choctaw")]
        Choctaw = 165,
        [Description("Chuukese")]
        Chuukese = 159,
        [Description("Croatian")]
        Croatian = 40,
        [Description("Czech")]
        Czech = 41,
        [Description("Danish")]
        Danish = 42,
        [Description("Dari")]
        Dari = 172,
        [Description("Dutch")]
        Dutch = 43,
        [Description("East Armenian")]
        East_Armenian = 44,
        [Description("Edo")]
        Edo = 45,
        [Description("English")]
        English = 7,
        [Description("Estonian")]
        Estonian = 46,
        [Description("Ewe")]
        Ewe = 47,
        [Description("Fijian")]
        Fijian = 48,
        [Description("Finnish")]
        Finnish = 49,
        [Description("French")]
        French = 8,
        [Description("Ga")]
        Ga = 126,
        [Description("Gagauz")]
        Gagauz = 38,
        [Description("Garifuna")]
        Garifuna = 125,
        [Description("Georgian")]
        Georgian = 50,
        [Description("German")]
        German = 51,
        [Description("Gilbertese")]
        Gilbertese = 52,
        [Description("Greek")]
        Greek = 53,
        [Description("Greenlandic")]
        Greenlandic = 54,
        [Description("Guarani")]
        Guarani = 129,
        [Description("Gujarati")]
        Gujarati = 114,
        [Description("Haitian Creole")]
        Haitian_Creole = 55,
        [Description("Hakha Chin")]
        Hakha_Chin = 121,
        [Description("Hawai'i Pidgin")]
        Hawaii_Pidgin = 168,
        [Description("Hawaiian")]
        Hawaiian = 166,
        [Description("Hebrew")]
        Hebrew = 56,
        [Description("Hindi")]
        Hindi = 9,
        [Description("Hmong")]
        Hmong = 10,
        [Description("Hmong (Green)")]
        Hmong_Green = 153,
        [Description("Hopi")]
        Hopi = 150,
        [Description("Hungarian")]
        Hungarian = 57,
        [Description("Icelandic")]
        Icelandic = 58,
        [Description("Iloko")]
        Iloko = 59,
        [Description("Indonesian")]
        Indonesian = 11,
        [Description("Irish")]
        Irish = 60,
        [Description("Isoko")]
        Isoko = 61,
        [Description("Italian")]
        Italian = 12,
        [Description("Jamaican Creole")]
        Jamaican_Creole = 167,
        [Description("Japanese")]
        Japanese = 13,
        [Description("Jarai")]
        Jarai = 144,
        [Description("Kabuverdianu")]
        Kabuverdianu = 156,
        [Description("Kannada")]
        Kannada = 131,
        [Description("Karen, Pwo")]
        Karen_Pwo = 118,
        [Description("Karen, Sgaw")]
        Karen_Sgaw = 117,
        [Description("Kinyarwanda")]
        Kinyarwanda = 149,
        [Description("Kirghiz")]
        Kirghiz = 170,
        [Description("Kirundi")]
        Kirundi = 148,
        [Description("Kongo")]
        Kongo = 62,
        [Description("Korean")]
        Korean = 14,
        [Description("Krio")]
        Krio = 146,
        [Description("Kurdish")]
        Kurdish = 122,
        [Description("Kurdish Sorani")]
        Kurdish_Sorani = 173,
        [Description("Kurmanji Kurdish")]
        Kurmanji_Kurdish = 63,
        [Description("Laotian")]
        Laotian = 15,
        [Description("Latvian")]
        Latvian = 64,
        [Description("Liberian English")]
        Liberian_English = 65,
        [Description("Lingala")]
        Lingala = 66,
        [Description("Lithuanian")]
        Lithuanian = 67,
        [Description("Luganda")]
        Luganda = 68,
        [Description("Luvale")]
        Luvale = 69,
        [Description("Maasai")]
        Maasai = 70,
        [Description("Macedonian")]
        Macedonian = 71,
        [Description("Malagasy")]
        Malagasy = 171,
        [Description("Malay")]
        Malay = 120,
        [Description("Malayalam")]
        Malayalam = 72,
        [Description("Maltese")]
        Maltese = 73,
        [Description("Mam")]
        Mam = 157,
        [Description("Mandarin")]
        Mandarin = 74,
        [Description("Mandinka")]
        Mandinka = 163,
        [Description("Mapudungun")]
        Mapudungun = 133,
        [Description("Marshallese")]
        Marshallese = 75,
        [Description("Mauritian Creole")]
        Mauritian_Creole = 76,
        [Description("Mien")]
        Mien = 115,
        [Description("Miskito")]
        Miskito = 77,
        [Description("Mixtec")]
        Mixtec = 139,
        [Description("Mongolian")]
        Mongolian = 78,
        [Description("Moore")]
        Moore = 161,
        [Description("Myanmar")]
        Myanmar = 79,
        [Description("Navajo")]
        Navajo = 80,
        [Description("Nepali")]
        Nepali = 81,
        [Description("Nigerian Pidgin")]
        Nigerian_Pidgin = 155,
        [Description("Niuean")]
        Niuean = 82,
        [Description("Norwegian")]
        Norwegian = 83,
        [Description("Nuer")]
        Nuer = 84,
        [Description("PA German")]
        PA_German = 136,
        [Description("Pashto")]
        Pashto = 37,
        [Description("Persian")]
        Persian = 16,
        [Description("Pidgin (West Africa)")]
        Pidgin_West_Africa = 151,
        [Description("Polish")]
        Polish = 85,
        [Description("Ponapean")]
        Ponapean = 154,
        [Description("Portuguese")]
        Portuguese = 17,
        [Description("Portuguese Creole")]
        Portuguese_Creole = 87,
        [Description("Portuguese (Portugal)")]
        Portuguese_Portugal = 86,
        [Description("Pular")]
        Pular = 160,
        [Description("Punjabi")]
        Punjabi = 88,
        [Description("Q'anjob'al")]
        Qanjobal = 137,
        [Description("Quiche")]
        Quiche = 164,
        [Description("Quichua")]
        Quichua = 134,
        [Description("Rohingya")]
        Rohingya = 169,
        [Description("Romanian")]
        Romanian = 89,
        [Description("Romany")]
        Romany = 90,
        [Description("Russian")]
        Russian = 18,
        [Description("Samoan")]
        Samoan = 19,
        [Description("Serbian")]
        Serbian = 91,
        [Description("Sign Language")]
        Sign_Language = 1,
        [Description("Sindhi")]
        Sindhi = 92,
        [Description("Sinhala")]
        Sinhala = 158,
        [Description("Slovak")]
        Slovak = 93,
        [Description("Slovenian")]
        Slovenian = 94,
        [Description("Solomon Islands Pidgin")]
        Solomon_Islands_Pidgin = 95,
        [Description("Somali")]
        Somali = 96,
        [Description("Sorani Kurdish")]
        Sorani_Kurdish = 97,
        [Description("Spanish")]
        Spanish = 20,
        [Description("Swahili")]
        Swahili = 98,
        [Description("Swedish")]
        Swedish = 99,
        [Description("Tagalog")]
        Tagalog = 21,
        [Description("Tahitian")]
        Tahitian = 100,
        [Description("Tajiki")]
        Tajiki = 127,
        [Description("Tamil")]
        Tamil = 119,
        [Description("Tarascan")]
        Tarascan = 124,
        [Description("Telugu")]
        Telugu = 130,
        [Description("Tetun Dili")]
        Tetun_Dili = 147,
        [Description("Tewa")]
        Tewa = 145,
        [Description("Thai")]
        Thai = 22,
        [Description("Tibetan")]
        Tibetan = 101,
        [Description("Tiddim Chin")]
        Tiddim_Chin = 128,
        [Description("Tigrinya")]
        Tigrinya = 102,
        [Description("Tongan")]
        Tongan = 103,
        [Description("Triqui")]
        Triqui = 140,
        [Description("Tswana")]
        Tswana = 104,
        [Description("Turkish")]
        Turkish = 105,
        [Description("Tuvaluan")]
        Tuvaluan = 106,
        [Description("Twi")]
        Twi = 107,
        [Description("Ukrainian")]
        Ukrainian = 108,
        [Description("Urdu")]
        Urdu = 109,
        [Description("Uzbek")]
        Uzbek = 110,
        [Description("Vietnamese")]
        Vietnamese = 23,
        [Description("Welsh")]
        Welsh = 111,
        [Description("Wolof")]
        Wolof = 162,
        [Description("Yoruba")]
        Yoruba = 135,
        [Description("Zapotec")]
        Zapotec = 142,
        [Description("Zulu")]
        Zulu = 113,
    }

    public static class LanguageExtensions
    {
        public static string ToDescriptiveString(this Language language)
            => Utils.EnumToDescriptiveString(language);
    }

    public static class LanguageUtils
    {
        public static Language FromValue(string value)
        {
            if(int.TryParse(value, out var langId))
            {
                if(Enum.IsDefined(typeof(Language), langId))
                {
                    return (Language)langId;
                }
            }

            return Language.UnsupportedLanguage;
        }

        public static Language FromDescription(string description)
            => Utils.OptionDescriptionToEnumValue<Language>(description);
    }
}
