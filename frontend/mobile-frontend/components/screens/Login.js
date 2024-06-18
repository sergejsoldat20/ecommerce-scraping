import React, { useEffect, useState } from "react";
import {
  View,
  Text,
  Button,
  StatusBar,
  StyleSheet,
  Dimensions,
  ActivityIndicator,
  FlatList,
} from "react-native";
import ProductCard from "../parts/ProductCard";
import productsService from "../../services/products.service";
import { useFilters } from "../../context/FilterContext";
export default function Login(props) {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const pageSize = 30;
  const { filters } = useFilters();

  let filterData = {
    brandId: "00000000-0000-0000-0000-000000000000",
    shopName: "",
    priceFrom: 0,
    priceTo: 0,
    gender: "",
  };

  useEffect(() => {
    console.log(filters);
    getProducts();
  }, [pageNumber]);

  const getProducts = () => {
    productsService
      .getProductsPageable(filters, pageNumber, pageSize)
      .then((result) => {
        // debugger;
        const newPageData = result?.data.items;
        setData([...data, ...newPageData]);
        setIsLoading(false);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const loadMoreItems = () => {
    setPageNumber(pageNumber + 1);
  };

  const renderLoader = () => {
    return isLoading ? (
      <View style={styles.loaderStyle}>
        <ActivityIndicator size="large" color="#aaa" />
      </View>
    ) : null;
  };

  const numColumns = Dimensions.get("window").width > 600 ? 4 : 2;
  return (
    <View
      style={{
        flex: 1,
        flexDirection: "column",
        paddingTop: StatusBar.currentHeight,
      }}
    >
      <View style={styles.container}>
        <FlatList
          data={data}
          onEndReached={loadMoreItems}
          onEndReachedThreshold={0}
          keyExtractor={(item) => item?.id}
          numColumns={numColumns}
          ListFooterComponent={renderLoader}
          renderItem={({ item }) => (
            <ProductCard product={item} navigator={props.navigation} />
          )}
        />
      </View>

      {/* <Button
        title="Idi na prvi ekran"
        onPress={() => props.navigation.navigate("Home")}
        color="gray"
      /> */}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 0,
    backgroundColor: "white",
  },
  loaderStyle: {
    marginVertical: 16,
    alignItems: "center",
  },
});
