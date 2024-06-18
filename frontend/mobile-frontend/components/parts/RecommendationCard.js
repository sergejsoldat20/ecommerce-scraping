import React from "react";
import { View, Text, Pressable, Image, StyleSheet } from "react-native";
import { scale } from "react-native-size-matters";
import { appColors } from "../../utils/appColors";
import Label from "./Label";
import { useNavigation } from "@react-navigation/native";

export default function RecommendationCard({ navigation, product }) {
  const realNavigation = useNavigation();

  const storeMappings = {
    "Buzz": "Shop1",
    "Sport Vision": "Shop2",
    "Sport Reality": "Shop3",
    "Juventa": "Shop4",
    "InnSport": "Shop5"
};

  const getStore = (storeName) => {
    return storeMappings[storeName] || "Unknown store";
  }

  const standardizePrice = (price) => price.toFixed(2);

  return (
    <View
      style={{
        margin: 5,
        padding: 10,
        width: "12%",
        backgroundColor: "white",
        alignContent: "center",
        justifyContent: "center",
        borderWidth: 1,
        borderColor: "#ccc",
        borderRadius: 10,
      }}
    >
      <Pressable
        onPress={() => {
          console.log(product?.id);
        }}
        style={{}}
      >
        {product.photoUrl &&
          product.photoUrl !== "" && ( // Add check for empty string
            <View>
              <Image
                source={{
                  uri: product.photoUrl,
                }}
                style={styles.image}
              />
              <Text style={styles.title}>{product?.name}</Text>
              <Text style={styles.shop}>{getStore(product?.shopName)}</Text>
            </View>
          )}

        {/* <View style={{ paddingVertical: scale(2) }}>
        <Label
          text={description?.substring(0, 24)}
          style={{ fontSize: scale(13), color: appColors.darkGray }}
        />
      </View> */}

        <View style={{ paddingVertical: scale(5) }}>
          <Label
            text={standardizePrice(product?.price) + " BAM"}
            style={{
              fontSize: scale(18),
              color: appColors.primary,
              fontWeight: "500",
            }}
          />
        </View>
      </Pressable>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 10,
    // Adjust the width as needed
  },
  image: {
    width: "100%",
    aspectRatio: 1, // Maintain aspect ratio
    resizeMode: "cover",
  },
  title: {
    marginTop: 8,
    fontSize: 16,
    fontWeight: "bold",
  },
  shop: {
    marginTop: 8,
    fontSize: 16,
    fontWeight: "bold",
    color: appColors.redOrange,
  },
  price: {
    marginTop: 8,
    fontSize: 18,
    color: "#00C510",
  },
});
