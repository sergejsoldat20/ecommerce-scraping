import React, { useState } from "react";
import { View, StyleSheet } from "react-native";
import Container from "../parts/Container";
import { scale } from "react-native-size-matters";
import { appColors, shadow } from "../../utils/appColors";
import Label from "../parts/Label";
import CustomInput from "../parts/CustomInput";
import CustomButton from "../parts/CustomButton";
import accountService from "../../services/account.service";

const SignUp = (props) => {
  const [payload, setPayload] = useState({});

  const onSubmit = () => {
    console.log(payload);
    accountService
      .register(payload)
      .then((response) => {
        if (response.status === 200) {
          props.navigation.navigate("SignIn");
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
            text="Registracija"
            style={{ fontSize: scale(30), fontWeight: "700" }}
          />
        </View>

        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("firstName", text)}
            keyboardType="default"
            label="Ime"
            // placeholder="New Balance A1234"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("lastName", text)}
            keyboardType="default"
            label="Prezime"
            // placeholder="New Balance A1234"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("email", text)}
            keyboardType="email-address"
            label="Email"
            // placeholder="New Balance A1234"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("username", text)}
            keyboardType="email-address"
            label="Korisnicko ime"
            // placeholder="New Balance A1234"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChange("password", text)}
            secureTextEntry
            label="Lozinka"
            // placeholder="New Balance A1234"
          />
        </View>

        <CustomButton
          // isLoading={isloading}
          onPress={onSubmit}
          label="Registruj se"
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

export default SignUp;
