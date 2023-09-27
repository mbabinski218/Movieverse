import { Dispatch, RefObject, SetStateAction, useEffect } from "react";

export const useOutsideClickAlerter = (ref: RefObject<HTMLDivElement>, callback: () => void) => {
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as HTMLInputElement)) {
        callback();
      }
    }

    document.addEventListener("click", handleClickOutside);
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };        
  }, [ref]);
}