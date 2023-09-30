import { useCallback, useState } from "react";
import { FilteredMediaDto } from "../../core/dtos/media/filteredMediaDto";
import { MediaDemo } from "./MediaDemo";
import "./MediaHorizontalList.css";

interface MediaHorizontalListProps {
  filteredMedia: FilteredMediaDto;
}

type ScrollButton = {
  left: boolean;
  right: boolean;
}

export const MediaHorizontalList: React.FC<MediaHorizontalListProps> = ({filteredMedia}) => {
  const [scrollButton, setScrollButton] = useState<ScrollButton>({left: false, right: true});
  const [scrollPosition, setScrollPosition] = useState<number>(0);

  const leftButtonScrollClick = useCallback(() => {
    setScrollPosition(scrollPosition - 100);
    setScrollButton({left: scrollPosition - 100 <= 0, right: scrollPosition + 100 >= 100});
  }, []);

  const rightButtonScrollClick = useCallback(() => {
    setScrollPosition(scrollPosition + 100);
    setScrollButton({left: scrollPosition - 100 <= 0, right: scrollPosition + 100 >= 100});
  }, []);


  return (
    <div>
      <span className="platfromName">{filteredMedia.platformName}</span>
      <div className="mediaHorizontalList">
        <button className="scrollButton" disabled={scrollButton.left} onClick={leftButtonScrollClick}>&lt;</button>
        <div className="mediaHorizontalListContainer" style={{transform: `translateX(${scrollPosition}%)`}}>
          {filteredMedia.media?.items?.map((media) => (
            <MediaDemo key={media.id} mediaDemo={media}/>
          ))}
        </div>
        <button className="scrollButton" disabled={scrollButton.right} onClick={rightButtonScrollClick}>&gt;</button>
      </div>
    </div>
  );
}