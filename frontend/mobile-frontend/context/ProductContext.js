// FilterContext.js
import React, { createContext, useContext, useState } from "react";
import productsService from "../services/products.service";

const ProductContext = createContext();

export const useProducts = () => useContext(ProductContext);

export const FilterProvider = ({ children }) => {
  const [currentProduct, setCurrentProduct] = useState({
    id: "",
    name: "",
    productUrl: "",
    photoUrl: "",
    brandId: "",
    price: 0,
    priceWithDiscount: 0,
    gender: 0,
  });

  const fetchProductById = (id) => {
    productsService
      .getProductById(id)
      .then((response) => {
        setCurrentProduct(response.data);
      })
      .catch((err) => {
        conole.log(err);
      });
  };

  return (
    <ProductContext.Provider value={{ currentProduct, fetchProductById }}>
      {children}
    </ProductContext.Provider>
  );
};
