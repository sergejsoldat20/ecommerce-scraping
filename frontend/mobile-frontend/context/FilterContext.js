// FilterContext.js
import React, { createContext, useContext, useState } from "react";

const FilterContext = createContext();

export const useFilters = () => useContext(FilterContext);

export const FilterProvider = ({ children }) => {
  const [filters, setFilters] = useState({
    brandId: "00000000-0000-0000-0000-000000000000",
    priceFrom: 0,
    priceTo: 0,
    shopName: "",
    gender: "",
    sortType: "BY_NAME",
    searchString: "",
  });

  const updateFilters = (newFilters) => {
    setFilters({ ...filters, ...newFilters });
  };

  return (
    <FilterContext.Provider value={{ filters, updateFilters }}>
      {children}
    </FilterContext.Provider>
  );
};
