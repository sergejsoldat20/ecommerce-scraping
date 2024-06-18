import { isValidElement } from "react";
import base from "./base.service";

const instance = base.service(false);

export const getProductsPageable = (filter, pageNumber, pageSize) => {
  console.log(filter);
  return instance.post(
    `/Products/filter?pageSize=${pageSize}&pageNumber=${pageNumber}`,
    filter,
  );
};

export const getProductById = (id) => {
  return instance.get(`/Products/get-by-id/${id}`);
};

export const getBrands = () => {
  return instance.get("/Products/brands");
};

export const getBrandNameById = (id) => {
  return instance.get(`/Products/brand-by-id/${id}`);
};

export const getShopNames = () => {
  return instance.get("/Products/shop-names");
};

export const getRecommendedProducts = (productId, productName) => {
  const recomendationsRequest = {
    id: productId,
    name: productName,
  };
  return instance.post(
    "/Products/product-recommendation",
    recomendationsRequest,
  );
};

export default {
  getProductsPageable,
  getProductById,
  getBrandNameById,
  getBrands,
  getShopNames,
  getRecommendedProducts,
};
