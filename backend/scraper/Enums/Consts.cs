using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scraper.Enums
{
    public class Consts
    {

        // Spider url's
        public static List<string> BuzzURLList = new List<string>
        {
            "https://www.buzzsneakers.ba/patike/za-muskarce+unisex/za-odrasle/",
            "https://www.buzzsneakers.ba/patike/za-zene+unisex/za-odrasle/"
        };

        public static List<string> SportVisionURLList = new List<string>
        {
            "https://www.sportvision.ba/patike/za-muskarce+unisex/za-odrasle/",
            "https://www.sportvision.ba/patike/za-zene+unisex/za-odrasle/"
        };

        public static List<string> SportRealityURLList = new List<string>
        {
            "https://www.sportreality.ba/patike/za-muskarce+unisex/za-odrasle/",
            "https://www.sportreality.ba/patike/za-zene+unisex/za-odrasle/"
        };

        public static string BuzzUrl = "https://www.buzzsneakers.ba";
        public static string SportVisionUrl = "https://www.sportvision.ba";
        public static string SportRealityUrl = "https://www.sportreality.ba";

        // public static string BuzzUrl = "https://www.sportvision.ba/obuca/za-muskarce+unisex/za-odrasle/";
        public static string BuzzProductsDiv = "item-data col-xs-12 col-sm-12";
        public static string BaseUrlBuzz = "https://www.buzzsneakers.ba";
        public static string AnanasUrl = "https://ananas.rs/kategorije/sport-i-rekreacija/sportska-garderoba/patike-za-sport";


        // JUVENTA CONSTS 

        // Add page number to the end
        public static string JuventaFemaleProductsUrl = "https://www.juventasport.com/getItems?category_id=9537&sex_id=9002&page=";
        public static string JuventaMaleProductsUrl = "https://www.juventasport.com/getItems?category_id=9537&sex_id[]=8983&page=";
        public static string JuventaBaseUrl = "https://www.juventasport.com";
        public static string JuventaSingleProductUrl = "https://www.juventasport.com/Product/";
        public static string JuventaName = "Juventa";
        public static string JuventaFemaleId = "9002";
        public static string JuventaMaleId = "8983";
        public static string JuventaAPIBaseURL = "https://www.juventasport.com/getItems";

        // INN SPORT CONSTS
        public static string InnSportProducts = "https://innsport.ba/collections/obuca/products.json?page=";

        public static string InnSportFemaleProductBaseURL = "https://innsport.ba/collections/zene/products/";
        public static string InnSportMaleProductBaseURL = "https://innsport.ba/collections/muskarci/products/";

        public static string InnSportMaleTag = "za-muskarce";
        public static string InnSportFemaleTag = "za-zene";
        public static string InnSportShopName = "InnSport";
    }
}