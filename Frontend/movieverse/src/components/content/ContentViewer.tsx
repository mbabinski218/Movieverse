import { useCallback, useEffect, useState } from "react";
import { useContent } from "../../hooks/useContent";
import { PageBlur } from "../basic/PageBlur";
import { ContentDto } from "../../core/dtos/content/contentDto";
import { Button } from "../basic/Button";
import { Loading } from "../basic/Loading";
import { CloudStore } from "../../CloudStore";
import { Player } from "../../common/playe";
import YouTube from "react-youtube";
import "./ContentViewer.css";
import Close from "../../assets/bars-close.svg";
import Blank from "../../assets/blank.png";
import ArrowLeft from "../../assets/arrow-left.svg";
import ArrowRight from "../../assets/arrow-right.svg";

export interface ContentProps {
  mediaId: string;
  onClose?: () => void;
}

export const ContentViewer: React.FC<ContentProps> = ({mediaId, onClose}) => {
  const [loading, setLoading] = useState<boolean>(true);
  const [content, setContent] = useContent(mediaId);
  const [currentContent, setCurrentContent] = useState<ContentDto | null>(null);
  const [currentIndex, setCurrentIndex] = useState<number>(0);

  useEffect(() => {
    if (content) {
      setCurrentContent(content[0]);
      setLoading(false);
    }
  }, [content]);

  const nextContentHandler = useCallback(() => {
    if (!currentContent) {
      return;
    }

    if (currentIndex < content!.length - 1) {
      const index = currentIndex + 1;
      setCurrentIndex(index);
      setCurrentContent(content![index]);
    }
    else {
      setCurrentIndex(0);
      setCurrentContent(content![0]);
    }
  }, [content, currentContent, currentIndex]);

  const prevContentHandler = useCallback(() => {
    if (!currentContent) {
      return;
    }

    if (currentIndex > 0) {
      const index = currentIndex - 1;
      setCurrentIndex(index);
      setCurrentContent(content![index]);
    }
    else {
      const index = content!.length - 1;
      setCurrentIndex(index);
      setCurrentContent(content![index]);
    }
  }, [content, currentContent, currentIndex]);

  return (
    <>      
      <PageBlur />
      {
        loading ? 
        <div className="content-viewer">
          <Loading />
        </div> :
        <>
          {
            content!.length > 0 ?            
            <>
              <div className="content-viewer">
                <span className="content-viewer-index">{`${currentIndex + 1}/${content!.length}`}</span>
                <img className="content-viewer-close" 
                     src={Close} 
                     alt="close" 
                     onClick={onClose} 
                />
                {
                  currentContent?.contentType === "video" ?
                  <div>
                    <YouTube videoId={currentContent?.path.split("v=")[1]}
                             iframeClassName="content-viewer-player"
                             opts={Player.opts}
                    />
                  </div> :
                  <div>
                    <img className="content-viewer-image"
                        src={currentContent ? CloudStore.getImageUrl(currentContent.path) : Blank}
                        alt={currentContent?.path}
                    />
                  </div>
                }
              </div>
              <Button className="content-viewer-prev"
                      imgSrc={ArrowLeft}
                      color="dark"
                      onClick={prevContentHandler}
              />
              <Button className="content-viewer-next"
                      imgSrc={ArrowRight}
                      color="dark"
                      onClick={nextContentHandler}
              />
            </> :
            <div className="content-empty">
              <img className="content-viewer-close" 
                   src={Close} 
                   alt="close" 
                   onClick={onClose} 
              />
              <span style={{margin: "10px"}}>No content</span>
            </div>
          }
        </>
      }
    </>
  )
}