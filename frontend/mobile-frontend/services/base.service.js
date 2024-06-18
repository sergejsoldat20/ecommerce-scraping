import axios from "axios";
import * as SecureStore from "expo-secure-store";
// import { useToken } from "../context/TokenContext";
const baseConfig = {
  baseURL: "http://192.168.100.15:5008",
};

// eslint-disable-next-line import/no-anonymous-default-export
export default {
  service: (useAuth) => {
    const instance = axios.create(baseConfig);
    instance.defaults.headers.common["Content-Type"] = "application/json";
    if (useAuth) {
      instance.interceptors.request.use(
        async (config) => {
          const token = await SecureStore.getItemAsync("token");
          if (token) {
            config.headers = {
              ...config.headers,
              Authorization: `Bearer ${token}`,
            };
          }
          return config;
        },
        (error) => {
          Promise.reject(error);
        },
      );
    }
    return instance;
  },
};
