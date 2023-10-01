import { useEffect, useState } from "react";

export const useLocalStorage = <T>(key: string, initialValue: T | (() => T)) => {
  const [storedValue, setStoredValue] = useState<T>(() => {
		try {
			const item = localStorage.getItem(key);

			if(item !== null) {
				return JSON.parse(item);
			}

			return typeof initialValue === "function" ? initialValue as () => T : initialValue;
		} 
    catch (err) {
      console.log(err);
      return initialValue;
    }
  });

  useEffect(() => {
    localStorage.setItem(key, JSON.stringify(storedValue));
  });

  return [storedValue, setStoredValue];
}