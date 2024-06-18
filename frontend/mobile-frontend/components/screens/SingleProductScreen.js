import {
  View,
  Text,
  StyleSheet,
  ImageBackground,
  Pressable,
  Dimensions,
  Linking,
} from "react-native";
import React, { useEffect } from "react";
import { scale } from "react-native-size-matters";
import productsService from "../../services/products.service";
import { useState } from "react";
import Container from "../parts/Container";
import Label from "../parts/Label";
import TitleComp from "../parts/TileComp";
import CustomButton from "../parts/CustomButton";
import { ScrollView } from "react-native";
import ProductCard from "../parts/ProductCard";
import { useNavigation } from "@react-navigation/native";
import { Button } from "react-native";
import RecommendationCard from "../parts/RecommendationCard";

export default function SingleProductScreen({ route }) {
  const realNavigation = useNavigation();
  const { id } = route.params;
  const [brand, setBrand] = useState(null);
  const [recommendations, setRecommendations] = useState([]);

  const [product, setProduct] = useState({
    id: "",
    name: "",
    productUrl: "",
    price: null,
    priceWithDiscount: null,
    shopName: "",
    photoUrl: "",
    gender: null,
    // oldValuesList: []
  });

  useEffect(() => {
    fetchProduct();
  }, []);

  const standardizePrice = (price) => price?.toFixed(2);

  const fetchProduct = () => {
    productsService
      .getProductById(id)
      .then((result) => {
        // console.log(result.data);
        setProduct(result.data);
        // fetch brand name by brandId
        productsService
          .getBrandNameById(result.data?.brandId)
          .then((result) => {
            setBrand(result.data);
          })
          .catch((err) => {
            console.log(err);
          });

        productsService
          .getRecommendedProducts(result.data?.id, result.data?.name)
          .then((response) => {
            //  console.log(response.data);
            setRecommendations(response.data);
          })
          .catch((err) => {
            console.log(err);
          });
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const checkGender = () => {
    if (product.gender === 0) {
      return "Za muskarce";
    } else {
      return "Za zene";
    }
  };

  const checkPriceWithDiscount = () => {
    if (
      product.priceWithDiscount != 0 &&
      product.priceWithDiscount != product.price
    ) {
      return true;
    }
    return false;
  };
  const onPress = () => {
    Linking.openURL(product.productUrl)
      .then(() => console.log("Link opened successfully"))
      .catch((err) => console.error("Error opening link:", err));
  };

  const screenWidth = Dimensions.get("window").width;
  const cardWidth = screenWidth * 0.48;

  return (
    <>
      <Container bodyStyle={{ paddingHorizontal: scale(0) }} isScrollable>
        <View>
          {product.photoUrl && (
            <ImageBackground
              style={{ height: scale(400), width: "100%" }}
              resizeMode="cover"
              source={{ uri: product?.photoUrl }}
            >
              <View
                style={{
                  marginTop: scale(40),
                  paddingHorizontal: scale(20),
                  flexDirection: "row",
                  justifyContent: "space-between",
                  alignItems: "center",
                }}
              ></View>
            </ImageBackground>
          )}
        </View>
        <View
          style={{ paddingHorizontal: scale(20), marginBottom: scale(100) }}
        >
          <View style={{ paddingVertical: scale(20) }}>
            <Label
              text={product?.name}
              style={{ fontWeight: "700", fontSize: scale(30) }}
            />
          </View>

          <View
            style={{
              paddingVertical: scale(10),
              flexDirection: "row",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          ></View>

          <View
            style={{
              paddingVertical: scale(10),
              flexDirection: "row",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <View style={styles.sizeContainer}>
              <Label text="Brend " style={{ fontSize: scale(15) }} />
              <Label
                text={brand?.name}
                style={{ fontWeight: "700", fontSize: scale(15) }}
              />
            </View>
          </View>
          <View
            style={{
              paddingVertical: scale(10),
              flexDirection: "row",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <View style={styles.sizeContainer}>
              <Label text="Prodavnica " style={{ fontSize: scale(15) }} />
              <Label
                text={product?.shopName}
                style={{ fontWeight: "700", fontSize: scale(15) }}
              />
            </View>
          </View>
          <View
            style={{
              paddingVertical: scale(10),
              flexDirection: "row",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <View style={styles.sizeContainer}>
              <Label text="Cijena " style={{ fontSize: scale(15) }} />
              <Label
                text={standardizePrice(product?.price) + " BAM"}
                style={{ fontWeight: "700", fontSize: scale(15) }}
              />
            </View>
          </View>
          {/* <Label
            text={product?.id}
            style={{ fontWeight: "700", fontSize: scale(15) }}
          /> */}
          {checkPriceWithDiscount() && (
            <View
              style={{
                paddingVertical: scale(10),
                flexDirection: "row",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <View style={styles.sizeContainer}>
                <Label
                  text="Cijena sa popustom "
                  style={{ fontSize: scale(15) }}
                />
                <Label
                  text={standardizePrice(product?.priceWithDiscount) + " BAM"}
                  style={{ fontWeight: "700", fontSize: scale(15) }}
                />
              </View>
            </View>
          )}

          <View
            style={{
              paddingVertical: scale(10),
              flexDirection: "row",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <View style={styles.sizeContainer}>
              <Label text="Pol " style={{ fontSize: scale(15) }} />
              <Label
                text={checkGender()}
                style={{ fontWeight: "700", fontSize: scale(15) }}
              />
            </View>
          </View>
          <View style={{ paddingVertical: scale(20) }}>
            <TitleComp heading={"Slicni proizvodi"} />
            <View>
              <ScrollView
                horizontal={true}
                //  showsVerticalScrollIndicator={false}
                // contentContainerStyle={{ paddingHorizontal: 10 }}
              >
                {recommendations.map((item, index) => (
                  <RecommendationCard
                    product={item}
                    navigator={realNavigation}
                    key={index}
                  />
                ))}
              </ScrollView>
            </View>
          </View>
          <View>
            <CustomButton
              //   isLoading={isloading}
              onPress={onPress}
              label="Kupi"
            />
          </View>
        </View>
      </Container>
    </>
  );
}

const styles = StyleSheet.create({
  sizeContainer: {
    flex: 1,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    backgroundColor: "white",
    padding: scale(10),
    paddingHorizontal: scale(20),
    borderRadius: scale(20),
    borderWidth: scale(0.4),
    borderColor: "#BEBEBE",
  },
  itemColor: {
    height: scale(20),
    width: scale(20),
    backgroundColor: "#00C569",
    borderRadius: scale(5),
  },
  wrtitle: {
    paddingVertical: scale(10),
    fontSize: scale(14),
    color: "#00C569",
  },
});
