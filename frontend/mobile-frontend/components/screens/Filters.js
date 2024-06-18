import React, { useEffect, useState } from "react";
import { View, TextInput, Button, StyleSheet, Pressable } from "react-native";
import Container from "../parts/Container";
import { scale } from "react-native-size-matters";
import { appColors, shadow } from "../../utils/appColors";
import Label from "../parts/Label";
import CustomInput from "../parts/CustomInput";
import CustomButton from "../parts/CustomButton";
import Dropdown from "../parts/Dropdown";
import { genders } from "../../utils/appConsts";
import DropDownPicker from "react-native-dropdown-picker";
import { useFilters } from "../../context/FilterContext";
import {
  getBrands,
  getProductById,
  getProductsPageable,
  getShopNames,
} from "../../services/products.service";

const Filters = (props) => {
  const { filters, updateFilters } = useFilters();

  const [brands, setBrands] = useState([]);
  const [shopList, setShopList] = useState([]);
  const [products, setProducts] = useState([]);

  const fetchBrands = () => {
    getBrands()
      .then((response) => {
        setBrands(response.data);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const fetchShopList = () => {
    getShopNames()
      .then((response) => {
        setShopList(response.data);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  useEffect(() => {
    fetchBrands();
    fetchShopList();
    //  console.log("Filters updated:", filters);
    // getProductsPageable(filters,1, 20).then((response) => {
    //   setProducts(response.data.items);
    //   console.log(response.data.items);
    // });
  }, [filters]);

  const [open, setOpen] = useState(false);
  const [brandValue, setBrandValue] = useState(
    "00000000-0000-0000-0000-000000000000",
  );

  const [genders, setGenders] = useState([
    { label: "Muskarci", value: "MALE" },
    { label: "Zene", value: "FEMALE" },
  ]);

  const [sortTypeValues, setSortTypeValues] = useState([
    { label: "Najvisa cijena", value: "BY_PRICE_DESC" },
    { label: "Najmanja cijena", value: "BY_PRICE_ASC" },
    { label: "Naziv", value: "BY_NAME" },
  ]);

  const [openShops, setOpenShops] = useState(false);
  const [shopsValue, setShopsValue] = useState("");

  const [openGenders, setOpenGenders] = useState(false);
  const [gendersValue, setGendersValue] = useState("");

  const [openSortType, setOpenSortType] = useState(false);
  const [sortValue, setSortValue] = useState(0);

  const [filterData, setFilterData] = useState({
    brandId: "00000000-0000-0000-0000-000000000000",
    priceFrom: 0,
    priceTo: 0,
    shopName: "",
    gender: "",
    sortType: 0,
    searchString: "",
  });
  const [isloading, setisloading] = useState(false);

  const onChangeText = (name, text) => {
    console.log("Name" + name + " ----- " + text);
    setFilterData({ ...filterData, [name]: text });
  };

  const onChangeNumbers = (name, number) => {
    setFilterData({ ...filterData, [name]: parseInt(number) });
  };

  const onSubmit = () => {
    filterData.shopName = shopsValue;
    filterData.gender = gendersValue;
    filterData.brandId = brandValue;
    filterData.sortType = sortValue;
    updateFilters(filterData);
    props.navigation.navigate("Druga");
  };

  const brandForDropDown = brands.map((brand) => ({
    label: brand.name,
    value: brand.id,
  }));

  const shopNamesForDropDown = shopList.map((shop) => ({
    label: shop,
    value: shop,
  }));

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
            text="Pretrazite proizvode"
            style={{ fontSize: scale(30), fontWeight: "700" }}
          />
        </View>

        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(text) => onChangeText("searchString", text)}
            keyboardType="default"
            label="Naziv"
            placeholder="New Balance A1234"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(num) => onChangeNumbers("priceFrom", num)}
            keyboardType="decimal-pad"
            label="Cijena od"
            placeholder="100"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <CustomInput
            onChangeText={(num) => onChangeNumbers("priceTo", num)}
            keyboardType="decimal-pad"
            label="Cijena do"
            placeholder="200"
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <View style={{ paddingVertical: scale(10) }}>
            <Label
              style={{ color: appColors.darkGray }}
              text="Tip sortiranja"
            />
          </View>

          <DropDownPicker
            style={{
              borderColor: appColors.darkGray,
              borderWidth: 1,
            }}
            placeholderStyle={{
              color: appColors.darkGray,
            }}
            placeholder={"Po nazivu"}
            // key={brand.id}
            open={openSortType}
            value={sortValue}
            items={sortTypeValues}
            setOpen={setOpenSortType}
            setValue={setSortValue}
            // setItems={setItems}
            onChangeValue={setSortValue}
            listMode="SCROLLVIEW" // Make the dropdown list scrollable
            dropDownDirection="BOTTOM" // Or 'TOP' or 'AUTO'
            maxHeight={200} // Set a maximum height for the dropdown list
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <View style={{ paddingVertical: scale(10) }}>
            <Label style={{ color: appColors.darkGray }} text="Brend" />
          </View>

          <DropDownPicker
            zIndex={open ? 3000 : openShops || openGenders ? 2000 : 1000}
            style={{
              borderColor: appColors.darkGray,
              borderWidth: 1,
            }}
            placeholderStyle={{
              color: appColors.darkGray,
            }}
            placeholder={"ADIDAS"}
            // key={brand.id}
            open={open}
            value={brandValue}
            items={brandForDropDown}
            setOpen={setOpen}
            setValue={setBrandValue}
            // setItems={setItems}
            onChangeValue={setBrandValue}
            listMode="SCROLLVIEW" // Make the dropdown list scrollable
            dropDownDirection="BOTTOM" // Or 'TOP' or 'AUTO'
            maxHeight={200} // Set a maximum height for the dropdown list
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <View style={{ paddingVertical: scale(10) }}>
            <Label style={{ color: appColors.darkGray }} text="Prodavnica" />
          </View>

          <DropDownPicker
            zIndex={openShops ? 3000 : open ? 2000 : openGenders ? 1000 : 500}
            style={{
              borderColor: appColors.darkGray,
              borderWidth: 1,
            }}
            placeholderStyle={{
              color: appColors.darkGray,
            }}
            placeholder={"Ponudjac 1"}
            open={openShops}
            value={shopsValue}
            items={shopNamesForDropDown}
            setOpen={setOpenShops}
            setValue={setShopsValue}
            // setItems={setShops}
            onChangeValue={setShopsValue}
            listMode="SCROLLVIEW" // Make the dropdown list scrollable
            dropDownDirection="BOTTOM" // Or 'TOP' or 'AUTO'
            maxHeight={200} // Set a maximum height for the dropdown list
          />
        </View>
        <View style={{ paddingVertical: scale(10) }}>
          <View style={{ paddingVertical: scale(10) }}>
            <Label style={{ color: appColors.darkGray }} text="Pol" />
          </View>
          <DropDownPicker
            zIndex={openGenders ? 3000 : open || openShops ? 2000 : 500}
            style={{
              borderColor: appColors.darkGray,
              borderWidth: 1,
            }}
            placeholderStyle={{
              color: appColors.darkGray,
            }}
            placeholder={"Muskarci"}
            open={openGenders}
            value={gendersValue}
            items={genders}
            setOpen={setOpenGenders}
            setValue={setGendersValue}
            setItems={setGenders}
            onChangeValue={setGendersValue}
            listMode="SCROLLVIEW" // Make the dropdown list scrollable
            dropDownDirection="BOTTOM" // Or 'TOP' or 'AUTO'
            maxHeight={200} // Set a maximum height for the dropdown list
          />
        </View>

        <CustomButton
          isLoading={isloading}
          onPress={onSubmit}
          label="Pretrazi"
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

export default Filters;
