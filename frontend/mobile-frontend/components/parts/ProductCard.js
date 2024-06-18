import { View, Text, Image, StyleSheet, TouchableOpacity } from "react-native";
import React from "react";
import { useNavigation } from "@react-navigation/native";
import { appColors } from "../../utils/appColors";

export default function ProductCard({ product, navigator }) {
  const realNavigator = useNavigation();
  const onPressProduct = () => {
    console.log("nav" + product.id);
    // debugger;
    realNavigator.navigate("ProductDetails", {
      id: product?.id,
    });
  };

  const standardizePrice = (price) => price.toFixed(2);

  return (
    <View style={styles.card}>
      <TouchableOpacity onPress={onPressProduct}>
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
              <Text style={styles.shop}>{product?.shopName}</Text>
              <Text style={styles.price}>
                {standardizePrice(product?.price) + " BAM"}
              </Text>
            </View>
          )}
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 10,
    padding: 10,
    margin: 8,
    width: "45%", // Adjust the width as needed
  },
  image: {
    width: "100%",
    aspectRatio: 1, // Maintain aspect ratio
    resizeMode: "cover",
  },
  shop: {
    marginTop: 8,
    fontSize: 12,
    fontWeight: "bold",
    color: appColors.redOrange,
  },
  title: {
    marginTop: 8,
    fontSize: 16,
    fontWeight: "bold",
  },
  price: {
    marginTop: 8,
    fontSize: 18,
    color: "#00C510",
  },
});
