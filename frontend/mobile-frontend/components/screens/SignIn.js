import React, { useState } from "react";
import { View, StyleSheet, Pressable } from "react-native";
import Container from "../parts/Container";
import { scale } from "react-native-size-matters";
import { appColors, shadow } from "../../utils/appColors";
import Label from "../parts/Label";
import CustomInput from "../parts/CustomInput";
import CustomButton from "../parts/CustomButton";
import accountService from "../../services/account.service";
import * as SecureStore from "expo-secure-store";

const SignIn = (props) => {
  const [payload, setPayload] = useState({});

  const onSubmit = () => {
    console.log(payload);
    accountService
      .login(payload)
      .then((response) => {
        if (response.status === 200) {
          // set JWT to contex
          SecureStore.setItemAsync("token", response.data.accessToken);
          props.navigation.navigate("Filters");
        }
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const onChange = (name, text) => {
    setPayload({ ...payload, [name]: text });
  };

  return (
    <Container isScrollable>
      <View
        style={{
          marginTop: scale(50),
          backgroundColor: appColors.white,
          ...shadow,
          padding: scale(15),
          borderRadius: scale(5),
        }}
      >
        <View
          style={{
            flexDirection: "row",
            justifyContent: "space-between",
            alignItems: "flex-end",
          }}
        >
          <Label
            text="Prijava"
            style={{ fontSize: scale(30), fontWeight: "700" }}
          />
          <Pressable onPress={() => props.navigation.navigate("SignUp")}>
            <Label
              text="Registracija"
              style={{
                fontSize: scale(14),
                fontWeight: "500",
                color: appColors.primary,
              }}
            />
          </Pressable>
        </View>

        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("email", text)}
            keyboardType="email-address"
            label="Email"
            placeholder="user@one.com"
            // value={"sergej.soldat@datascope-tech.com"}
            // placeholder="New Balance A1234"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("password", text)}
            secureTextEntry
            label="Lozinka"
            placeholder="passw0rd"
            // value={"passw0rd"}
          />
        </View>

        <CustomButton
          // isLoading={isloading}
          onPress={onSubmit}
          label="Prijavi se"
        />
      </View>
    </Container>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  input: {
    height: 40,
    margin: 12,
    borderWidth: 1,
    padding: 10,
    width: "80%", // Adjust as needed
  },
  priceContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    padding: "2px",
  },
});

export default SignIn;
