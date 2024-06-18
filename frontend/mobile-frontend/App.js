import { StatusBar } from "expo-status-bar";
import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { StyleSheet, Text, View, Button, useColorScheme } from "react-native";
import Login from "./components/screens/Login";
import { NavigationContainer } from "@react-navigation/native";

import { createStackNavigator } from "@react-navigation/stack";
import Products from "./components/screens/Products";
import SingleProductScreen from "./components/screens/SingleProductScreen";
import Filters from "./components/screens/Filters";
import { FilterProvider } from "./context/FilterContext";
import SignIn from "./components/screens/SignIn";
import SignUp from "./components/screens/SignUp";

const MainNavigator = createStackNavigator();
export default function App() {
  return (
    <FilterProvider>
      <NavigationContainer>
        <MainNavigator.Navigator
          initialRouteName="Home"
          screenOptions={{ presentation: "modal" }}
        >
          <MainNavigator.Screen
            name="Filters"
            component={Filters}
            options={{ title: "Filteri" }}
          />
          <MainNavigator.Screen
            name="Home"
            component={Products}
            options={{ title: "Pocetna strana" }}
          />
          <MainNavigator.Screen
            name="ProductDetails"
            component={SingleProductScreen}
            options={{ title: "Detalji o proizvodu" }}
          />
          <MainNavigator.Screen
            name="Druga"
            component={Login}
            options={{
              title: "Proizvodi",
            }}
          />
          <MainNavigator.Screen
            name="SignIn"
            component={SignIn}
            options={{
              title: "Prijava",
            }}
          />
          <MainNavigator.Screen
            name="SignUp"
            component={SignUp}
            options={{
              title: "Registracija",
            }}
          />
        </MainNavigator.Navigator>
      </NavigationContainer>
    </FilterProvider>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "white",
    alignItems: "center",
    justifyContent: "center",
  },
});
