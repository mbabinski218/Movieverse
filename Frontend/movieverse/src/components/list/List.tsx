import { ChangeEvent, useEffect, useState } from "react";
import { useTheme } from '@mui/material/styles';
import { PaginatedList } from "../../core/types/paginatedList";
import { ThemeProvider } from "react-bootstrap";
import Pagination from '@mui/material/Pagination';
import "./List.css";

export interface ListItem {
  id: string;
  label: string;
  stats: string | null;
  description: string | null;
  image: string | null;
}

export interface ListProps {
  loading: boolean;
  element: React.FC<ListItem>;
  list: PaginatedList<ListItem>;
  pageSize: number;
  onPageChange?: ((event: ChangeEvent<unknown>, page: number) => void);
}

export const List: React.FC<ListProps> = ({loading, element: Element, list, pageSize, onPageChange}) => {
  const theme = useTheme();

  useEffect(() => {
    theme.palette.text.primary = "#ffffff";
    theme.palette.primary.main = "#ffffff";
    theme.shape.borderRadius = 10;
  }, []);

  return (
    <div className={loading ? "list-loading" : "list-style"}>
      {
        !loading &&
        <>
          <div>
            {
              list.items.length !== 0 &&
              list.items.map((item, index) => {
                return (
                  <div key={item.id}>
                    <Element id={item.id}
                            label={`${index + 1 + (pageSize * (list.pageNumber ? list.pageNumber - 1 : 0 ))}. ${item.label}`}
                            stats={item.stats}
                            description={item.description}
                            image={item.image}
                    />
                    {
                      index !== list.items.length - 1 &&
                      <hr className="break"/>
                    }
                  </div>
                )
              }) 
            }
            {
              list.items.length === 0 &&
              <div className="list-empty">
                <span>No results</span>
              </div>
            }
          </div>
          <ThemeProvider theme={theme}>
            <Pagination className="list-pagination"
                        shape="rounded"
                        color="primary"
                        variant="outlined"
                        count={list.totalPages ?? 1}
                        page={list.pageNumber ?? 1}
                        onChange={onPageChange}
            />
          </ThemeProvider>
        </>
      }
    </div>
  )
}