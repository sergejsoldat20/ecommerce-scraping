import { View, Text, Button, StatusBar, TouchableOpacity } from "react-native";
import React, { useEffect, useState } from "react";
import axios from "axios";
import CustomButton from "../parts/CustomButton";
export default function Products(props) {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Define the API endpoint URL
    const apiUrl = "http://192.168.1.3:5008/Products/shop-names"; // Replace with your API URL

    // Make a GET request using Axioss
    axios
      .get(apiUrl)
      .then((response) => {
        // Handle the successful response and update the state
        setData(response.data);
        setIsLoading(false);
      })
      .catch((error) => {
        // Handle any errors that occurred during the request
        console.error("Error fetching data:", error);
        setIsLoading(false);
      });
  }, []);
  return (
    <View
      style={{
        flex: 1,
        flexDirection: "column",
        paddingTop: StatusBar.currentHeight,
      }}
    >
      <Text style={{ fontSize: 24, textAlign: "center" }}>
        Ovo je neki pocetni ekran aplikacije ...
      </Text>
      <Text>{data[0]}</Text>

      <CustomButton
        // isLoading={isloading}
        onPress={() => props.navigation.navigate("Druga")}
        label="Proizvodi"
      />
      <CustomButton
        //  isLoading={isloading}
        onPress={() => props.navigation.navigate("Filters")}
        label="Filteri"
      />

      <CustomButton
        //  isLoading={isloading}
        onPress={() => props.navigation.navigate("SignIn")}
        label="SignIn"
      />
      {/* <Button
        title="Idi na proizvode"
        onPress={() => props.navigation.navigate("Druga")}
      />
      <Button
        title="Idi na filtere"
        onPress={() => props.navigation.navigate("Filters")}
      /> */}
    </View>
  );
}
