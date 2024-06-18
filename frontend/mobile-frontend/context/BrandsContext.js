// FilterContext.js
import React, { createContext, useContext, useState } from "react";

const BrandContext = createContext();

export const useBrands = () => useContext(BrandContext);

export const BrandProvider = ({ children }) => {
    
    const [ brands, setBrands] = useState([]);

  return (
    <BrandContext.Provider value={{ brands, setBrands }}>
      {children}
    </BrandContext.Provider>
  );
};
